using VSFly.Models;

namespace VSFlyAPI.Models
{
    public class BookingM
    {
        public virtual int Id { get; set; }
        public virtual double SalePrice { get; set; }

        public int IdFlight { get; set; }
        public int IdPassenger { get; set; }


        // Foreing Key
        /*[ForeignKey("Flight")]
        [InverseProperty("BookedIn")]*/
        //public virtual Flight Flight { get; set; }

        /*[ForeignKey("Passenger")]
        [InverseProperty("Bookings")]*/
        //public virtual Passenger Passenger { get; set; }
    }
}
