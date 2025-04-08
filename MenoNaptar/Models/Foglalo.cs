using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenoNaptar.Models
{
    public class Foglalo
    {
        [Key]
        public string Szemszam { get; set; }
        public string Nev { get; set; }
        public int Iranyitoszam { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{Szemszam};{Nev};{Iranyitoszam};{Email}";
        }
    }
}
