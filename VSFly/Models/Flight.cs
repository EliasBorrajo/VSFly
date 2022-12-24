using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly.Models
{
    public class Flight
    {
        [Key]
        public virtual int FlightNo { get; set; }
        public virtual string Departure { get; set; }
        public virtual string Destination { get; set; }
        public virtual DateTime Date { get; set;   }
        public virtual int TotalSeats { get; set; }
        public virtual int FreeSeats { get; set; }
        public virtual double BasePrice { get; set; }

        // Clé étrangères
        public virtual ICollection<Booking> BookedIn { get; set; }
        public virtual ICollection<Employee> EmployeesOnBoard { get; set; }
    }
}
