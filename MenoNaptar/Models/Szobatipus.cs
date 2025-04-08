using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MenoNaptar.Models
{
    public class Szobatipus
    {
        [Key]
        public int SztId { get; set; }
        public int Ferohelyek { get; set; }

        [NotMapped]
        public BitmapImage Alaprajz { get; set; }

        [NotMapped]
        private string alaprajzKep;

        public string AlaprajzKep { get {
                return alaprajzKep;
            } 
            set {
                alaprajzKep = value;
                Alaprajz = new BitmapImage(new Uri(alaprajzKep, UriKind.RelativeOrAbsolute));
            } 
        }
    }
}
