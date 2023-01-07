using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VSFlyWebApp.Models;
using VSFlyWebApp.Services;

namespace VSFlyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVSFlyServices _flyService;

        /// <summary>
        /// Initializes a new instance of the HomeController class.
        /// </summary>
        /// <param name="logger">The logger used for logging.</param>
        /// <param name="flyService">The flight service used for retrieving flight information.</param>
        public HomeController(ILogger<HomeController> logger, IVSFlyServices flyService)
        {
            _logger = logger;
            _flyService = flyService;
        }

        /// <summary>
        /// Gets the list of available flights and displays it to the user.
        /// </summary>
        /// <returns>The list of available flights.</returns>
        public async Task<IActionResult> Index()
        {
            var flightList = await _flyService.GetAvailableFlights();
            return View(flightList);
        }

        /// <summary>
        /// Gets information about a specific flight and displays it to the user.
        /// </summary>
        /// <param name="id">The id of the flight to get information about.</param>
        /// <returns>Information about the specified flight.</returns>
        public async Task<IActionResult> Flight(int id)
        {
            List<double> results = new List<double>();

            var flightSalePrice = await _flyService.GetFlightSalePrice(id);
            results.Add(flightSalePrice);

            var totalSalePriceOfFlight = await _flyService.GetTotalSalePriceOfFlight(id);
            results.Add(totalSalePriceOfFlight);

            results.Add(id);

            return View(results);
        }

        /// <summary>
        /// Displays the booking form for a specific flight.
        /// </summary>
        /// <param name="id">The id of the flight to book.</param>
        /// <returns>The booking form for the specified flight.</returns>
        public IActionResult Booking(int id)
        {
            var booking = new Booking { idFlight = id };
            return View(booking);
        }

        /// <summary>
        /// Books a flight with the specified information.
        /// </summary>
        /// <param name="booking">The booking information for the flight.</param>
        /// <returns>A redirect to the list of available flights.</returns>
        [HttpPost]
        public async Task<IActionResult> Booking(Booking booking)
        {
            bool test = await _flyService.BookAFlight(booking.idPassenger, booking.idFlight);
            Console.WriteLine("HomeController : méthode Booking POST " + test);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets the average sale price of all flights to the specified destination and displays it to the user.
        /// </summary>
        /// <param name="destination">The destination to get the average sale price for.</param>
        /// <returns>The average sale price of all flights to the specified destination.</returns>
        public async Task<IActionResult> Details(string destination)
        {
            var averageSalePriceOfAllFlightsInDestinationTo = await _flyService.GetAverageSalePriceOfAllFlightsInDestinationTo(destination);
            return View(averageSalePriceOfAllFlightsInDestinationTo);
        }

        /// <summary>
        /// Gets a list of booked tickets for flights to the specified destination and displays it to the user.
        /// </summary>
        /// <param name="destination">The destination to get the booked tickets for.</param>
        /// <returns>A list of booked tickets for flights to the specified destination.</returns>
        public async Task<IActionResult> ResumeBookedTicketsOfDestination(string destination)
        {
            var bookedTickets = await _flyService.GetResumeBookedTicketsOfDestination(destination);
            return View(bookedTickets);
        }

        /// <summary>
        /// Displays the privacy policy to the user.
        /// </summary>
        /// <returns>The privacy policy.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the error page to the user.
        /// </summary>
        /// <returns>The error page.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}