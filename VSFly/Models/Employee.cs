using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly.Models
{
    public class Employee : Person
    {
        public virtual double Salary { get; set; }
        public virtual bool isPilot { get; set; }

        // Foreing Key
        public virtual ICollection<Flight> Flights { get; set; } 
        // Faire une vérification que on ne puisse pas le mettre au même flight à 2 dates identiques.
    }
}
