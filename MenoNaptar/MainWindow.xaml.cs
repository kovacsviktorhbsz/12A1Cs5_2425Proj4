using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MenoNaptar.Models;
using MenoNaptar.Database;
using System.Data.Entity;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices.ComTypes;

namespace MenoNaptar
{
    public struct FoglalasDate
    {
        public Button btn;
        public DateTime date;

        public FoglalasDate(Button btn, DateTime date)
        {
            this.btn = btn;
            this.date = date;
        }
    };

    public class FoglalasDates
    {
        public FoglalasDate? checkIn { get; set; }
        public FoglalasDate? checkOut { get; set; }

        public FoglalasDates(int _)
        {
            checkIn = null;
            checkOut = null;
        }

        public void Null()
        {
            checkIn = null;
            checkOut = null;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime currMonth { get; set; }
        public FoglalasDates selectedDates { get; set; }
        public DataContext Context { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Context = new DataContext();

            currMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            selectedDates = new FoglalasDates(0);

            generateCalendar();
        }

        private void generateCalendar()
        {
            naptarGrid.Children.Clear();
            naptarGrid.RowDefinitions.Clear();
            naptarGrid.ColumnDefinitions.Clear();

            int people;
            bool res = !int.TryParse(numPeople.Text, out people);

            if (res || people <= 0)
            {
                naptarGrid.RowDefinitions.Add(new RowDefinition());
                naptarGrid.ColumnDefinitions.Add(new ColumnDefinition());

                TextBlock tb = new TextBlock();
                tb.Text = "Töltsd ki a vendégek számát a fenti űrlapon!";
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.FontSize = 25;

                naptarGrid.Children.Add(tb);

                prevMonthBtn.IsEnabled = false;
                nextMonthBtn.IsEnabled = false;
                TBYearMonth.Text = "";

                selectedDates.Null();

                szobaKep.Source = null;
                CBAvailableRooms.SelectedItem = null;
                
                return;
            }

            prevMonthBtn.IsEnabled = true;
            nextMonthBtn.IsEnabled = true;

            var thisMonth = currMonth;

            TBYearMonth.Text = thisMonth.ToString("yyyy. MMMM");

            int days = DateTime.DaysInMonth(thisMonth.Year, thisMonth.Month);

            for (int i = 0; i < 7; i++)
            {
                naptarGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int dayOfTheWeek = (int)thisMonth.DayOfWeek == 0 ? 6 : (int)thisMonth.DayOfWeek - 1;
            int row = 0;
            naptarGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 1; i <= days; i++, dayOfTheWeek++)
            {
                if (dayOfTheWeek == 7)
                {
                    naptarGrid.RowDefinitions.Add(new RowDefinition());
                    dayOfTheWeek = 0;
                    row++;
                }

                var today = new DateTime(thisMonth.Year, thisMonth.Month, i);
                    
                Button btn = new Button();
                btn.Content = $"{i}";
                btn.Name = $"btn{i}";
                btn.Click += Btn_Click;

                
                var foglaltSzobak = Context.Foglalasok.Include(x => x.Szoba).Where(x => x.CheckInDatum <= today && today <= x.CheckOutDatum).Select(x => x.Szoba);
                var availableSzobatipusok = Context.Szobak.Include(x => x.Szt).GroupBy(x => x.Szt.SztId).ToDictionary(x => x.Key, x => x.ToList());
                foreach (var fsz in foglaltSzobak)
                {
                    availableSzobatipusok[fsz.SztId].Remove(fsz);
                }
                var avaSztId = availableSzobatipusok.Where(x => x.Value.Count != 0).Select(x => x.Key);
                var filteredAvailableSzobatipusok = avaSztId.Where(x => people <= Context.Szobatipusok.Where(y => x == y.SztId).FirstOrDefault().Ferohelyek).ToList();
                btn.IsEnabled = filteredAvailableSzobatipusok.Count != 0;

                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, dayOfTheWeek);

                naptarGrid.Children.Add(btn);
            }
        }

        private bool BtnAction(Action<Button> func)
        {
            var dayBtns = naptarGrid.Children.OfType<Button>().Where(
                    x => selectedDates.checkIn.Value.date.Day <= int.Parse((string)x.Content) &&
                    int.Parse((string)x.Content) <= selectedDates.checkOut.Value.date.Day);

            foreach (var dayBtn in dayBtns)
            {
                try
                {
                    func(dayBtn);
                } catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            updateAvailableRooms();
            var btn = (Button)sender;
            DateTime date = new DateTime(currMonth.Year, currMonth.Month, int.Parse((string)btn.Content));

            if (selectedDates.checkIn.HasValue && selectedDates.checkOut.HasValue)
            {
                BtnAction(x => x.Background = SystemColors.ControlLightBrush);
                selectedDates.Null();
            }

            updateAvailableRooms();

            if (!selectedDates.checkIn.HasValue)
            {
                selectedDates.checkIn = new FoglalasDate(btn, date);
                btn.Background = Brushes.LightBlue;
                return;
            }

            if (date <= selectedDates.checkIn.Value.date)
            {
                selectedDates.checkIn.Value.btn.Background = SystemColors.ControlLightBrush;
                selectedDates.checkIn = null;
                MessageBox.Show("Az érkezés nem lehet később, mint a távozás!", "Hiba");
                return;
            }

            selectedDates.checkOut = new FoglalasDate(btn, date);

            if (!BtnAction(x => {
                if (!x.IsEnabled)
                    throw new Exception();

                x.Background = Brushes.LightBlue;
            }))
            {
                BtnAction(x =>
                {
                    x.Background = SystemColors.ControlLightBrush;
                });
                selectedDates.Null();
                MessageBox.Show("Ez alatt az időtartam alatt már van foglalás!", "Hiba");
            }

            updateAvailableRooms();
        }

        private void updateAvailableRooms()
        {
            if (selectedDates.checkIn == null || selectedDates.checkOut == null)
            {
                CBAvailableRooms.IsEnabled = false;
                szobaKep.Source = null;
                return;
            }

            DateTime checkIn = selectedDates.checkIn.Value.date;
            DateTime checkOut = selectedDates.checkOut.Value.date;

            int people;
            bool res = !int.TryParse(numPeople.Text, out people);

            if (res || people <= 0)
            {
                return;
            }


            var foglaltSzobak = Context.Foglalasok.Include(x => x.Szoba).Where(x => !((x.CheckInDatum < checkIn && x.CheckOutDatum < checkIn) ||
                                                      (x.CheckInDatum > checkOut && x.CheckOutDatum > checkOut))).Select(x => x.Szoba);

            var fittingSzobatipusok = Context.Szobatipusok.Where(x => people <= x.Ferohelyek);

            var fittingSzobak = Context.Szobak.Include(x => x.Szt).Where(x => fittingSzobatipusok.Contains(x.Szt)).Where(x => !foglaltSzobak.Contains(x));

            CBAvailableRooms.Items.Clear();
            
            foreach (var szoba in fittingSzobak)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = $"{szoba.Szobaszam}";

                CBAvailableRooms.Items.Add(cbi);
            }

            CBAvailableRooms.SelectedItem = null;

            CBAvailableRooms.IsEnabled = true;
        }

        private void nextMonthBtn_Click(object sender, RoutedEventArgs e)
        {
            currMonth = currMonth.AddMonths(1);
            selectedDates.Null();
            generateCalendar();
        }

        private void prevMonthBtn_Click(object sender, RoutedEventArgs e)
        {
            currMonth = currMonth.AddMonths(-1);
            selectedDates.Null();
            generateCalendar();
        }

        private void DataTextChanged(object sender, TextChangedEventArgs e)
        {
            generateCalendar();
        }

        private void CBAvailableRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int roomNum;
            if (CBAvailableRooms.SelectedItem == null || 
                !int.TryParse((string)((ComboBoxItem)CBAvailableRooms.SelectedItem).Content, out roomNum))
            {
                foglalasBtn.IsEnabled = false;
                return;
            }

            var szobatipus = Context.Szobak.Where(x => x.Szobaszam == roomNum).Select(x => x.Szt).FirstOrDefault();

            if (szobatipus != null) {
                szobaKep.Source = szobatipus.Alaprajz;

                foglalasBtn.IsEnabled = true;
            }
        }

        private void foglalasBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TbSzemszam.Text.Trim().Length != 8 || TbNev.Text.Length == 0 || TbEmail.Text.Length == 0 || TbIsz.Text.Length == 0)
            {
                MessageBox.Show("Ki kell tölteni az adatokat a foglaláshoz!", "Hiba!");
                return;
            }

            int isz;
            if (!int.TryParse(TbIsz.Text, out isz) || !TbEmail.Text.Contains("@") || !TbEmail.Text.Contains("."))
            {
                MessageBox.Show("Az adatoknak érvényesnek kell lennie a foglaláshoz!", "Hiba!");
                return;
            }

            if (CBAvailableRooms.SelectedItem == null)
            {
                return;
            }

            int people;
            bool res = !int.TryParse(numPeople.Text, out people);
            if (res || people <= 0)
            {
                return;
            }

            int szobaszam = int.Parse((string)((ComboBoxItem)CBAvailableRooms.SelectedItem).Content);
            int ellatas = int.Parse((string)((ComboBoxItem)ellatasLevel.SelectedItem).Tag);

            var szoba = Context.Szobak.Where(x => x.Szobaszam == szobaszam).FirstOrDefault();

            if (szoba == null)
            {
                // Azt hogy...

                MessageBox.Show("Hiba! Nincs ilyen szoba.");
                return;
            }

            var foglalo = Context.Foglalok.Where(x => x.Szemszam == TbSzemszam.Text).FirstOrDefault();

            if (foglalo == null)
            {
                foglalo = Context.Foglalok.Add(new Foglalo
                {
                    Szemszam = TbSzemszam.Text,
                    Nev = TbNev.Text,
                    Iranyitoszam = isz,
                    Email = TbEmail.Text
                });
            } else
            {
                foglalo.Nev = TbNev.Text;
                foglalo.Iranyitoszam = isz;
                foglalo.Email = TbEmail.Text;
            }

            Context.Foglalasok.Add(new Foglalas { 
                Szoba = szoba,
                Foglalo = foglalo,
                CheckInDatum = selectedDates.checkIn.Value.date,
                CheckOutDatum = selectedDates.checkOut.Value.date,
                Ellatas = ellatas,
                SzemelyekSzama = people,
            });

            Context.SaveChanges();

            Reset();

            MessageBox.Show("Sikeres foglalás", "Woo-Hoo!");
        }

        private void Reset()
        {
            TbSzemszam.Text = "";
            TbEmail.Text = "";
            TbIsz.Text = "";
            TbNev.Text = "";

            TbNev.IsEnabled = false;
            TbIsz.IsEnabled = false;
            TbEmail.IsEnabled = false;

            numPeople.Text = "";
            ellatasLevel.SelectedIndex = 0;

            CBAvailableRooms.SelectedItem = null;

            generateCalendar();
            updateAvailableRooms();
        }

        private void adminBtn_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            AdminPage adminPage = new AdminPage(Context);
            adminPage.ShowDialog();
        }

        private void TbSzemszam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbSzemszam.Text.Trim().Length == 8)
            {
                TbNev.IsEnabled = true;
                TbIsz.IsEnabled = true;
                TbEmail.IsEnabled = true;

                var szemely = Context.Foglalok.Where(x => x.Szemszam == TbSzemszam.Text).FirstOrDefault();

                if (szemely != null)
                {
                    TbNev.Text = szemely.Nev;
                    TbIsz.Text = $"{szemely.Iranyitoszam}";
                    TbEmail.Text = szemely.Email;
                }
            } else
            {
                TbNev.IsEnabled = false;
                TbIsz.IsEnabled = false;
                TbEmail.IsEnabled = false;

                TbNev.Text = "";
                TbIsz.Text = "";
                TbEmail.Text = "";
            }
        }
    }
}
