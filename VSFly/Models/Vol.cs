using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly.Models
{
    public class Vol
    {
        [Key]
        public virtual int IdVol { get; set; }
        public virtual string Depart { get; set; }
        public virtual string Destination { get; set; }
        public virtual DateTime DateVol { get; set; }
        public virtual int SiegesTotaux { get; set; }
        public virtual int SiegesVides { get; set; }
        public virtual float PrixTicketBase { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
