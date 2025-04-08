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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MenoNaptar.Models;
using MenoNaptar.Database;
using System.Data.Entity;
using System.Windows.Media.Animation;

namespace MenoNaptar
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        private enum SelectedTable {
            None = -1,
            Foglalasok = 0,
            Foglalok = 1
        }

        public DataContext Context { get; set; }
        private SelectedTable SelTable { get; set; }

        public AdminPage(DataContext data)
        {
            InitializeComponent();

            Context = data;
            SelTable = SelectedTable.None;

            //DG.DataContext = filterFoglalasok(Context.Foglalasok.Include(x => x.Foglalo).Include(x => x.Szoba).ToList(), "");
        }

        private void DGFoglalasok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            torlesBtn.IsEnabled = (Foglalas)DGFoglalasok.SelectedItem != null;
        }

        private List<Foglalas> filterFoglalasok(List<Foglalas> foglalasok, string filter)
        {
            return foglalasok.ToList().Where(x => x.ToString().ToLower().Contains(filter.ToLower())).ToList();
        }

        private List<Foglalo> filterFoglalok(List<Foglalo> foglalok, string filter)
        {
            return foglalok.ToList().Where(x => x.ToString().ToLower().Contains(filter.ToLower())).ToList();
        }

        private void TbKereses_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (SelTable)
            {
                case SelectedTable.Foglalasok:
                    DGFoglalasok.DataContext = filterFoglalasok(Context.Foglalasok.Include(x => x.Foglalo).Include(x => x.Szoba).ToList(), TbKereses.Text);
                    break;
                case SelectedTable.Foglalok:
                    DGFoglalok.DataContext = filterFoglalok(Context.Foglalok.ToList(), TbKereses.Text);
                    break;
                default:
                    return;
            }
        }

        private void torlesBtn_Click(object sender, RoutedEventArgs e)
        {
            

            switch (SelTable)
            {
                case SelectedTable.Foglalasok:
                    if (MessageBox.Show("Biztosan törlöd?", "Törlés?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        return;

                    var kijeloltFoglalas = (Foglalas)DGFoglalasok.SelectedItem;

                    if (kijeloltFoglalas == null)
                        return;

                    Context.Foglalasok.Remove(kijeloltFoglalas);
                    Context.SaveChanges();

                    DGFoglalasok.DataContext = filterFoglalasok(Context.Foglalasok.Include(x => x.Foglalo).Include(x => x.Szoba).ToList(), TbKereses.Text);
                    break;
                case SelectedTable.Foglalok:
                    var kijeloltFoglalo = (Foglalo)DGFoglalok.SelectedItem;

                    if (kijeloltFoglalo == null)
                        return;

                    var count = Context.Foglalasok.Include(x => x.Foglalo).Where(x => x.Foglalo.Szemszam == kijeloltFoglalo.Szemszam).Count();

                    if (count == 0)
                    {
                        if (MessageBox.Show("Biztosan törlöd?", "Törlés?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            return;
                    } else
                    {
                        MessageBox.Show("Ehhez a foglalóhoz tartoznak foglalások, előbb törölje azokat.", "Hiba");
                        return;
                    }

                    Context.Foglalok.Remove(kijeloltFoglalo);
                    Context.SaveChanges();

                    DGFoglalok.DataContext = filterFoglalok(Context.Foglalok.ToList(), TbKereses.Text);
                    break;
                default:
                    return;
            }
        }

        private void CBTabla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBTabla.SelectedIndex == 0)
            {
                DGFoglalok.Visibility = Visibility.Collapsed;
                DGFoglalasok.Visibility = Visibility.Visible;
                SelTable = SelectedTable.Foglalasok;
                DGFoglalasok.DataContext = filterFoglalasok(Context.Foglalasok.Include(x => x.Foglalo).Include(x => x.Szoba).ToList(), TbKereses.Text);
                DGFoglalasok.SelectedItem = null;
            }

            if (CBTabla.SelectedIndex == 1)
            {
                DGFoglalasok.Visibility = Visibility.Collapsed;
                DGFoglalok.Visibility = Visibility.Visible;
                SelTable = SelectedTable.Foglalok;
                DGFoglalok.DataContext = filterFoglalok(Context.Foglalok.ToList(), TbKereses.Text);
                DGFoglalok.SelectedItem = null;
            }
        }

        private void DGFoglalok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SelTable)
            {
                case SelectedTable.Foglalasok:
                    torlesBtn.IsEnabled = (Foglalas)DGFoglalasok.SelectedItem != null;
                    break;
                case SelectedTable.Foglalok:
                    torlesBtn.IsEnabled = (Foglalo)DGFoglalok.SelectedItem != null;
                    break;
                default:
                    return;
            }
        }

        private void DGFoglalok_CurrentCellChanged(object sender, EventArgs e)
        {
            Context.SaveChanges();
        }

        private void DGFoglalasok_CurrentCellChanged(object sender, EventArgs e)
        {
            Context.SaveChanges();
        }
    }
}
