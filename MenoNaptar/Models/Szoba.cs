using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenoNaptar.Models
{
    public class Szoba
    {
        [Key]
        public int Szobaszam { get; set; }
        public int SztId { get; set; }
        public virtual Szobatipus Szt { get; set; }
    }
}
