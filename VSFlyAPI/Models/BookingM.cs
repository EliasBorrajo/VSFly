using VSFly.Models;

namespace VSFlyAPI.Models
{
    public class BookingM
    {
        public virtual int Id { get; set; }
        public virtual double SalePrice { get; set; }

        public int IdFlight { get; set; }
        public int IdPassenger { get; set; }


    }
}
