using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly.Models
{
    public class Reservation
    {
        [Key]
        public virtual int IdReservation { get; set; }
        public virtual float PrixTicketEffectif { get; set; }
        public virtual Passager Passager { get; set; }
        public virtual Vol Vol { get; set; }

    }
}
