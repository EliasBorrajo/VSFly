using VSFlyWebApp.Models;

namespace VSFlyWebApp.Services
{
    public interface IVSFlyServices
    {
        Task<IEnumerable<Flight>> GetAvailableFlights();
    }
}