using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSFly.Models;

namespace VSFly
{
    public class AeroportContext : DbContext
    {
        public DbSet<Vol> Vols { get; set; }
        public DbSet<Passager> Passagers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public static string ConnectionString { get; set; } = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\uadmin\\Documents\\ProgrammationComposants\\VSFlyDatabase\\Aeroport.mdf;Integrated Security=True;Connect Timeout=30";
        public AeroportContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseLazyLoadingProxies();
            builder.UseSqlServer(ConnectionString);
        }
    }
}
