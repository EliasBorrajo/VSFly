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

        public HomeController(ILogger<HomeController> logger, IVSFlyServices flyService)
        {
            _logger = logger;
            _flyService = flyService;
        }

        public async Task<IActionResult> Index()
        {
            var flightList = await _flyService.GetAvailableFlights();
            return View(flightList);
        }

        public async Task<IActionResult> Details(string destination)
        {
            var averageSalePriceOfAllFlightsInDestinationTo = await _flyService.GetAverageSalePriceOfAllFlightsInDestinationTo(destination);
            return View(averageSalePriceOfAllFlightsInDestinationTo);
        }

        public async Task<IActionResult> ResumeBookedTicketsOfDestination(string destination)
        {
            var bookedTickets = await _flyService.GetResumeBookedTicketsOfDestination(destination);
            return View(bookedTickets);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}