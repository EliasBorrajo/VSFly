using VSFly.Models;

namespace VSFlyAPI.Models
{
    /// <summary>
    /// Represents a booking with information including the ID, sale price, flight ID, and passenger ID.
    /// </summary>
    public class BookingM
    {
        public virtual int Id { get; set; }
        public virtual double SalePrice { get; set; }

        public int IdFlight { get; set; }
        public int IdPassenger { get; set; }


    }
}
