using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly.Models
{
    public class Booking
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual double SalePrice { get; set; }

        // Foreing Key
        //[ForeignKey("FlightNo")]
        [ForeignKey("Flight")]
        public int IdFlight { get; set; }
        public virtual Flight Flight { get; set; }

        //[ForeignKey("Id")]
        [ForeignKey("Passenger")]
        public int IdPassenger { get; set; }
        public virtual Passenger Passenger { get; set; }
    }
}
