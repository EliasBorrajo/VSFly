namespace VSFlyAPI.Models
{
    /// <summary>
    /// Represents a flight with information including flight number, departure and destination locations, date, total and available seats, and base price.
    /// </summary>
    public class FlightM
    {
        public virtual int FlightNo { get; set; }
        public virtual string Departure { get; set; }
        public virtual string Destination { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int TotalSeats { get; set; }
        public virtual int FreeSeats { get; set; }
        public virtual double BasePrice { get; set; }
    }
}
