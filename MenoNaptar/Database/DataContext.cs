using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenoNaptar.Models;

namespace MenoNaptar.Database
{
    public class DataContext : DbContext
    {
        public DbSet<Foglalas> Foglalasok { get; set; }
        public DbSet<Szoba> Szobak { get; set; }
        public DbSet<Szobatipus> Szobatipusok { get; set; }
        public DbSet<Foglalo> Foglalok { get; set; }
        public DataContext() : base("name=SzobafoglaloContext") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
