using VSFlyWebApp.Models;

namespace VSFlyWebApp.Services
{
    public interface IVSFlyServices
    {
        Task<IEnumerable<Flight>> GetAvailableFlights();

        Task<double> GetFlightSalePrice(int id);

        Task<double> GetTotalSalePriceOfFlight(int id);

        Task<double> GetAverageSalePriceOfAllFlightsInDestinationTo(string Destination);

        Task<IEnumerable<Models.ResumeBookedTicket>> GetResumeBookedTicketsOfDestination(string destination);
    }
}