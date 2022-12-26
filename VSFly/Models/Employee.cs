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
        public virtual Flight Flight { get; set; } 
        // L'employé n'est attribué qu'à un seul vol à la fois.
    }
}
