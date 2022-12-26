using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly.Models
{
    public class Passenger : Person
    {
        public virtual bool isActiveClient { get; set; }

        // Foreing Key
        // Flight <---------------- Booking -------------------> Passenger
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
