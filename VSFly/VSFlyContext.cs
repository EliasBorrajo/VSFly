using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSFly.Models;

namespace VSFly
{
    public class VSFlyContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public static string ConnectionString { get; set; } = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\uadmin\\Documents\\ProgrammationComposants\\VSFlyDatabase\\Aeroport.mdf;Integrated Security=True;Connect Timeout=30";
        //public static string ConnectionString { get; set; } = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\garam\\Desktop\\Documents\\HES-SO\\HEG\\CSharp\\S5\\Composants_cours\\04_Projet_VSFlight\\DataBase\\DB_VsFly.mdf;Integrated Security=True;Connect Timeout=30";

        /// <summary>
        /// Initializes a new instance of the VSFlyContext class.
        /// </summary>
        public VSFlyContext() { }

        //Est appelé au début quand on lance l'application.
        /// <summary>
        /// Configures the options for the VSFlyContext.
        /// </summary>
        /// <param name="builder">The options builder to configure.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {             
            // Elements de configuration de Entity Framework
            builder.UseLazyLoadingProxies();
            builder.UseSqlServer(ConnectionString);   // On lui dit ou est la DB
        }
    }
}
