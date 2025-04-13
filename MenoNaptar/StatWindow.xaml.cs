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
using System.Windows.Shapes;
using MenoNaptar.Database;
using MenoNaptar.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace MenoNaptar
{
    /// <summary>
    /// Interaction logic for StatWindow.xaml
    /// </summary>
    public partial class StatWindow : Window
    {
        private DataContext Context { get; set; }

        [DbFunction("MySql", "DATEDIFF")]
        private int? DiffDays(DateTime? endDate, DateTime? startDate)
        {
            throw new NotImplementedException();
        }

        public StatWindow(DataContext context)
        {
            InitializeComponent();

            Context = context;

            // Legtobbet foglalt szobak
            var legtFog = Context.Foglalasok.Include(x => x.Szoba)
                                              .GroupBy(x => x.Szobaszam)
                                              .Max(x => x.Count());

            var legtFogSzobak = Context.Foglalasok.Include(x => x.Szoba).GroupBy(x => x.Szobaszam).Where(x => x.Count() == legtFog);

            var legtFogSzobakKi = "";

            foreach (var legtFogSzoba in legtFogSzobak)
            {
                legtFogSzobakKi += $"{legtFogSzoba.Key} ({legtFog}x)  ";
            }

            TBlegFogSzoba.Text = legtFogSzobakKi;

            // Leghosszabb foglalas
            TBlegFog.Text = $"{Context.Foglalasok.Max(x => this.DiffDays(x.CheckOutDatum, x.CheckInDatum))} nap";

            // Atlag foglalas hossz
            TBatlFogl.Text = $"{Context.Foglalasok.Average(x => this.DiffDays(x.CheckOutDatum, x.CheckInDatum))} nap";

            // Legtobbet foglalo szemely
            var szemLegtFog = Context.Foglalasok.Include(x => x.Foglalo)
                                                .GroupBy(x => x.Foglalo)
                                                .Max(x => x.Count());

            var legtFogSzemelyek = Context.Foglalasok.Include(x => x.Foglalo).GroupBy(x => x.Foglalo).Where(x => x.Count() == szemLegtFog);

            var legtFogSzemelyekKi = "";

            foreach (var legtFogSzem in legtFogSzemelyek)
            {
                legtFogSzemelyekKi += $"{legtFogSzem.Key.Szemszam} ({szemLegtFog}x)  ";
            }

            TBlegFogSzem.Text = legtFogSzemelyekKi;

            // Atlag szemelyek szama
            TBatlSzem.Text = $"{Context.Foglalasok.Average(x => x.SzemelyekSzama)}";
        }
    }
}
