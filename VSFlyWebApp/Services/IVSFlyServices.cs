using VSFlyWebApp.Models;

namespace VSFlyWebApp.Services
{
    public interface IVSFlyServices
    {
        /// <summary>
        /// Gets a list of available flights from the API.
        /// </summary>
        /// <returns>A list of available flights.</returns>
        Task<IEnumerable<Flight>> GetAvailableFlights();

        /// <summary>
        /// Gets the sale price of a specific flight from the API.
        /// </summary>
        /// <param name="id">The id of the flight to get the sale price for.</param>
        /// <returns>The sale price of the specified flight.</returns>
        Task<double> GetFlightSalePrice(int id);

        /// <summary>
        /// Gets the total sale price of a specific flight from the API.
        /// </summary>
        /// <param name="id">The id of the flight to get the total sale price for.</param>
        /// <returns>The total sale price of the specified flight.</returns>
        Task<double> GetTotalSalePriceOfFlight(int id);

        /// <summary>
        /// Gets the average sale price of all flights to a specific destination from the API.
        /// </summary>
        /// <param name="destination">The destination to get the average sale price for.</param>
        /// <returns>The average sale price of all flights to the specified destination.</returns>
        Task<double> GetAverageSalePriceOfAllFlightsInDestinationTo(string Destination);

        /// <summary>
        /// Gets a list of booked tickets for flights to a specific destination from the API.
        /// </summary>
        /// <param name="destination">The destination to get the booked tickets for.</param>
        /// <returns>A list of booked tickets for flights to the specified destination.</returns>
        Task<IEnumerable<Models.ResumeBookedTicket>> GetResumeBookedTicketsOfDestination(string destination);

        /// <summary>
        /// Books a flight with the specified passenger and flight id using the API.
        /// </summary>
        /// <param name="idPassenger">The id of the passenger booking the flight.</param>
        /// <param name="idFlight">The id of the flight being booked.</param>
        /// <returns>True if the flight was successfully booked, false otherwise.</returns>
        Task<bool> BookAFlight(int idPassenger, int idFlight);
    }
}