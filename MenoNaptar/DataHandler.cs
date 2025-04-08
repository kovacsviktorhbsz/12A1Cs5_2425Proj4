using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenoNaptar.Models;
using System.IO;

namespace MenoNaptar
{
    public class DataHandler
    {
        public List<Foglalas> Foglalasok { get; set; }
        public List<Szobatipus> Szobatipusok { get; set; }
        public Szobak Szobak { get; set; }

        public DataHandler() 
        {
            try
            {
                Foglalasok = Foglalas.LoadFromFile("../../Assets/DataFiles/Foglalasok.txt");
            } catch (Exception)
            {
                Foglalasok = new List<Foglalas>();
            }
            Szobatipusok = Szobatipus.LoadFromFile("../../Assets/DataFiles/Szobatipusok.txt");
            Szobak = Szobak.LoadFromFile("../../Assets/DataFiles/Szobak.txt");
        }

        public void WriteFoglalasok()
        {
            List<string> lines = new List<string>();
            lines.Add(File.ReadAllLines("../../Assets/DataFiles/Foglalasok.txt").First());
            foreach (var foglalasLine in Foglalasok)
            {
                lines.Add(foglalasLine.ToString());
            }
            File.WriteAllLines("../../Assets/DataFiles/Foglalasok.txt", lines);
        }
    }
}
