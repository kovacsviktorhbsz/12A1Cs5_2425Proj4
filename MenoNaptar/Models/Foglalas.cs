using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenoNaptar.Models
{
    public enum EllatasTipus
    {
        Nincs = 0,
        Reggeli = 1,
        ReggeliVacsora = 2,
        AllInclusive = 3,
    };

    public class Foglalas
    {
        [Key]
        public int FoglalasId { get; set; }
        public int Szobaszam { get; set; }
        public virtual Szoba Szoba { get; set; }
        public string FoglaloSzemszam { get; set; }
        public virtual Foglalo Foglalo { get; set; }
        public DateTime CheckInDatum { get; set; }
        public DateTime CheckOutDatum { get; set; }
        public int SzemelyekSzama { get; set; }
        [Column(TypeName = "int")]
        public EllatasTipus Ellatas { get; set; }

        public override string ToString()
        {
            return $"{Szobaszam};{Foglalo};{CheckInDatum.ToString("yyyy-MM-dd")};{CheckOutDatum.ToString("yyyy-MM-dd")};{SzemelyekSzama};{Ellatas}";
        }
    }
}
