namespace VSFlyAPI.Models
{
    /// <summary>
    /// Represents a summary of a booked ticket including the passenger's name, flight number, and sale price.
    /// </summary>
    public class ResumeBookedTicket
    {
        public string PassengerName { get; set; }
        public int FlightNumber { get; set; }
        public double TicketSalePrice { get; set; }
    }
}
