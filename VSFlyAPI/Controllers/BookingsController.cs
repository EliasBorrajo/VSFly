using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using VSFly;
using VSFly.Models;
using VSFlyAPI.Extension;
using VSFlyAPI.Models;

namespace VSFlyAPI.Controllers
{
    /// <summary>
    /// The BookingsController class is a controller that handles requests related to bookings in the VSFlyAPI.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly VSFlyContext _context;

        public BookingsController(VSFlyContext context)
        {
            _context = context;
        }

        // M A I N   C O M M A N D S   -   R E Q U I R E M E N T S   S P E C I F I C A T I O N S 

        // GET: api/Bookings/AvailableFlights
        /// <summary>
        /// Handles a GET request to the "api/Bookings/AvailableFlights" endpoint.
        /// Returns a list of all available flights (not full).
        /// </summary>
        /// <returns>A list of available flights.</returns>
        [HttpGet("AvailableFlights")]
        public async Task<ActionResult<IEnumerable<FlightM>>> GetAvailableFlights()
        {
            // a.	Return all available flights (not full)
            var flights = await _context.Flights.Where(x => x.Date > DateTime.Now && x.FreeSeats > 0 ).ToListAsync();
            var flightsMAPI = new List<FlightM>();

            foreach(var flight in flights )
            {
                flightsMAPI.Add(  flight.ConvertToFlightMAPI()  ); ;
            }

            return flightsMAPI;
        }

        // GET: api/Bookings/FlightSalePrice/IdFlight
        /// <summary>
        /// Handles a GET request to the "api/Bookings/FlightSalePrice/{idFlight}" endpoint.
        /// Returns the sale price of a flight with the specified ID.
        /// </summary>
        /// <param name="idFlight">The ID of the flight to get the sale price for.</param>
        /// <returns>The sale price of the specified flight.</returns>
        [HttpGet("FlightSalePrice/{idFlight}")]
        public async Task<ActionResult<double>> FlightSalePrice(int idFlight)
        {
            // b.	Return the sale price of a flight

            // 1) Get the flight
            var flight = await _context.Flights.FindAsync(idFlight);

            if (flight == null)
            {
                return NotFound();
            }

            // 2) Calculate his price
            return CalculateSalePrice(flight);
        }

        // POST: api/Bookings/BookAFlight/1/2
        /// <summary>
        /// Handles a POST request to the "api/Bookings/BookAFlight/{idPassenger}/{idFlight}" endpoint.
        /// Buys a ticket on a flight for the specified passenger and flight.
        /// </summary
        [HttpPost("BookAFlight/{idPassenger}/{idFlight}")]
        public async Task<ActionResult> BookAFlight(int idPassenger ,int idFlight)
        {
            Console.WriteLine("API Values = " + idPassenger + " " + idFlight);
            // c.	Buying a ticket on a flight
            // 1) recuperer les entités depuis la DB
            var passenger = await _context.Passengers.FindAsync( idPassenger );
            var flight = await _context.Flights.FindAsync( idFlight );

            if(passenger == null || flight == null)
            {
                return NotFound();
            }

            // 2) Vérifier que le vol ait encore de la place
            if(flight.FreeSeats <= 0)
            {
                return BadRequest("Sorry, there are no more seats available on this flight.");
            }

            // 3) Créer un nouveau booking pour le vol & passager
            Booking booking = new Booking();
            booking.Passenger = passenger;
            booking.Flight = flight;
            booking.SalePrice = CalculateSalePrice(flight);

            _context.Bookings.Add(booking);

            // 4) Mettre à jour le vol
            /* Exemple : 
             * context.Entry(entity).State = EntityState.Modified;
             * 
             * Synchrone   : context.SaveChanges();
             * Asynchrone : await context.SaveChangesAsync();
             */
            flight.FreeSeats--;
            //flight.BookedIn.Add(booking);                                               // Ajouter le booking à la liste de booking du flight
            _context.Entry(flight).State = EntityState.Modified;


            // 5) Mettre à jour le passager
            //passenger.Bookings.Add(booking);
            _context.Entry(passenger).State = EntityState.Modified;

            // 6) Save
            await _context.SaveChangesAsync();  

            return Ok("Booking created ! ");
        }

        // GET: api/Bookings/TotalSalePriceOfFlight/IdFlight
        /// <summary>
        /// Returns the total sale price of all tickets sold for a flight.
        /// </summary>
        /// <param name="idFlight">The ID of the flight.</param>
        /// <returns>The total sale price of all tickets for the flight as a double.</returns>
        [HttpGet("TotalSalePriceOfFlight/{idFlight}")]
        public async Task<ActionResult<double>> TotalSalePriceOfFlight(int idFlight)
        {
            // d.	Return the total sale price of all tickets sold for a flight
            // 1) Get the flight
            var flight = await _context.Flights.FindAsync(idFlight);

            if (flight == null)
            {
                return NotFound();
            }

            // 2) Calculate his price
            double result = 0.0;
            if(flight.BookedIn.Count > 0)
                result = flight.BookedIn.Sum(x => x.SalePrice);
        
            return result;
        }

        // GET: api/Bookings/AverageSalePriceOfAllFlightsInDestinationTo/Destination
        /// <summary>
        /// Returns the average sale price of all tickets sold for a destination.
        /// </summary>
        /// <param name="Destination">The destination for which to calculate the average sale price of tickets.</param>
        /// <returns>The average sale price of tickets for the destination as a double.</returns>
        [HttpGet("AverageSalePriceOfAllFlightsInDestinationTo/{Destination}")]
        public async Task<ActionResult<double>> AverageSalePriceOfAllFlightsInDestinationTo (string Destination)
        {
            // e.	Return the average sale price of all tickets sold for a destination (multiple flights possible)
            // 1) input verification
            if( ! _context.Flights.Any( x =>  x.Destination.Equals(Destination) )  ) // Si aucun vol n'a cette destination
            {
                return BadRequest("No destination found for any of our flights.");
            }

            // 2) Get the flights for a certain destination
            var flightList = await _context.Flights.Where( x => Destination.Equals(x.Destination) ).ToListAsync();
            var bookingsForDestination = new List<Booking>();

            if (flightList == null)
            {
                return NotFound();
            }

            // Ajouter chaque booking de chaque vol à une liste de bookings
            foreach (var flight in flightList)
            {
                bookingsForDestination.AddRange( flight.BookedIn );
            }

            // 3) Calculate his price
            double result;

            result = bookingsForDestination.Any() ? 
                                                                             bookingsForDestination.Average( x=> x.SalePrice) 
                                                                             : 0.0;
            return result;
        }

        // GET: api/Bookings/ResumeBookedTickets/Destination
        /// <summary>
        /// Returns a list of all tickets sold for a destination with the first and last name of the travelers and the flight number as well as the sale price of each ticket.
        /// </summary>
        /// <param name="Destination">The destination for which to return ticket information.</param>
        /// <returns>A list of objects containing information about tickets sold for the destination.</returns>
        [HttpGet("ResumeBookedTicketsOfDestination/{Destination}")]
        public async Task<ActionResult<IEnumerable<ResumeBookedTicket>>> ResumeBookedTicketsOfDestination(string Destination)
        {
            //  f.	Return the list of all tickets sold for a destination with the first and last name of the travelers
            //  and the flight number as well as the sale price of each ticket

            // 1) input verification
            if (!_context.Flights.Any(x => x.Destination.Equals(Destination))) // Si aucun vol n'a cette destination
            {
                return BadRequest("No destination found for any of our flights.");
            }

            // 2) Get the flights for a certain destination
            var flightsForDestination = await _context.Flights.Where(x => Destination.Equals(x.Destination)).ToListAsync();
            var resumeTicketsList = new List<ResumeBookedTicket>();

            if (flightsForDestination == null)
            {
                return NotFound();
            }

            // Ajouter chaque booking de chaque vol à une liste de bookings
            foreach (var flight in flightsForDestination)
            {
                foreach (var booking in flight.BookedIn)
                {
                    resumeTicketsList.Add( booking.ConvertToResumeTicketMAPI() );
                }
            }

            return resumeTicketsList;

        }




        // CREATE & DELETE DB INPUTS
        // POST: api/Bookings/NewPassenger/name/isActive/email
        /// <summary>
        /// Creates a new passenger in the database.
        /// </summary>
        /// <param name="name">The name of the passenger.</param>
        /// <param name="isActive">A boolean indicating whether the passenger is active or not.</param>
        /// <param name="email">The email of the passenger.</param>
        /// <returns></returns>
        [HttpPost("NewPassenger/{name}/{isActive}/{email}")]
        public async Task<ActionResult> NewPassenger( string name, bool isActive, string email )
        {
            // 1) Input verifications
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Name or Email null or empty");
            }
            if (name.Length > 30 || email.Length > 50)
            {
                return BadRequest("Name or Email too long");
            }
            if (isActive == null)
            {
                return BadRequest("IsActive cannot be null.");
            }
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            {
                return BadRequest("Email not valid");
            }

            // 2) Créer un nouveau passager pour la DB
            Passenger passenger = new Passenger();
            passenger.Name = name;
            passenger.Email = email;
            passenger.isActiveClient = isActive;

            _context.Passengers.Add(passenger);

            // 6) Save
            _context.SaveChanges();

            return Ok("Passenger created ! ");
        }

        // POST: api/Bookings/NewFlight/params...
        /// <summary>
        /// Creates a new booking in the database.
        /// </summary>
        /// <param name="flightId">The ID of the flight for the booking.</param>
        /// <param name="passengerId">The ID of the passenger for the booking.</param>
        /// <param name="salePrice">The sale price of the ticket for the booking.</param>
        /// <returns></returns>
        [HttpPost("NewFlight/{departure}/{destination}/{date_YMD}/{basePrice}/{totalSeats}/{freeSeats}")]
        public async Task<ActionResult> NewFlight(string departure, string destination, DateTime date_YMD, double basePrice, int totalSeats, int freeSeats)
        {
            // Validate input
            if (string.IsNullOrEmpty(departure))
            {
                return BadRequest("Departure cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(destination))
            {
                return BadRequest("Destination cannot be null or empty.");
            }
            if (date_YMD == null)
            {
                return BadRequest("Date cannot be null. \nFormat must be : \nDate Time  = Year, Month, Day, Hour, Minutes, Seconds");
            }
            if (date_YMD == DateTime.MinValue)
            {
                return BadRequest("Date is not a valid date and time.");
            }
            if (date_YMD == DateTime.MaxValue)
            {
                return BadRequest("Date cannot be set to the maximum value.");
            }
            if (basePrice <= 0)
            {
                return BadRequest("Base price cannot be negative or 0.");
            }
            if (totalSeats <= 0)
            {
                return BadRequest("Total seats must be greater than zero.");
            }
            if (freeSeats < 0 || freeSeats > totalSeats)
            {
                return BadRequest("Free seats must be between 0 and total seats.");
            }

            // Create the Flight
            Flight flight = new Flight() { Departure = departure,
                                                         Destination = destination,
                                                         Date = date_YMD,
                                                         BasePrice = basePrice,
                                                         TotalSeats = totalSeats,
                                                         FreeSeats = freeSeats };

            _context.Add(flight);

            // create a random pilot for the plane
            var pilot = new Employee() { Name = "Arthur_AutoGenerated", isPilot = true, Email = departure+"_"+destination+"@gmail.com", Salary = 5000.00, Flight = flight };

            _context.Add(pilot);

            _context.SaveChanges();

            return Ok("Flight and his Pilot created ! ");

        }

        // M Y   M E T H O D S 
        /// <summary>
        /// Calculates the percentage of seats that are filled on a flight.
        /// </summary>
        /// <param name="flight">The flight for which to calculate the filling percentage.</param>
        /// <returns>The percentage of seats that are filled on the flight.</returns>
        double CalculateFillingseats(Flight flight)
        {
            return ((double)flight.TotalSeats - (double)flight.FreeSeats) / (double)flight.TotalSeats * 100.00;
        }

        /// <summary>
        /// Calculates the sale price for a flight based on the filling percentage of the seats and the number of months before the departure date.
        /// </summary>
        /// <param name="flight">The flight for which to calculate the sale price.</param>
        /// <returns>The sale price for the flight.</returns>
        double CalculateSalePrice(Flight flight)
        {
            // % seats fillled = (total - free) / total * 100
            double FillingSeatsPercent = CalculateFillingseats(flight);
            Console.WriteLine("Filling Seats % = " + FillingSeatsPercent);

            if (FillingSeatsPercent >= 80)
            {
                Console.WriteLine("1.\tIf the airplane is more than 80% full regardless of the date. \ta. sale price = 150% of the base price\r\n");
                return flight.BasePrice * 1.5;
            }
            else if (FillingSeatsPercent < 20 &&
                        NbrMonthsBeforeDeparture(flight.Date, DateTime.Now) < 2)
            {
                Console.WriteLine("2.\tIf the plane is filled less than 20% less than 2 months before departure: \ta. sale price = 80% of the base price\r\n");
                return flight.BasePrice * 0.8;
            }
            else if (FillingSeatsPercent < 50 &&
                        NbrMonthsBeforeDeparture(flight.Date, DateTime.Now) < 1)
            {
                Console.WriteLine("3.\tIf the plane is filled less than 50% less than 1 month before departure: \ta. sale price = 70% of the base price\r\n");
                return flight.BasePrice * 0.7;
            }
            else
            {
                Console.WriteLine("4.\tIn all other cases: \ta. sale price = base price\r\n");
                return flight.BasePrice;
            }
        }

        /// <summary>
        /// method first checks if the years of the two dates are the same. 
        /// If they are, the method calculates the absolute difference between the months in the same way as before. 
        /// If the years are different, the method calculates the number of months remaining in the first year 
        /// and adds 12 months for each additional year between the two dates
        /// </summary>
        int NbrMonthsBeforeDeparture(DateTime FlightDate, DateTime ActualDate)
        {
            int monthsbetween = 0;

            if (FlightDate.Year == ActualDate.Year)
            {
                monthsbetween = Math.Abs(FlightDate.Month - ActualDate.Month);
            }
            else
            {
                int yearsBetween = Math.Abs(FlightDate.Year - ActualDate.Year);
                int monthsInFirstYear = 12 - ActualDate.Month;
                monthsbetween = monthsInFirstYear;

                for (int i = 1; i < yearsBetween; i++)
                {
                    monthsbetween += 12;
                }

            }

            Console.WriteLine("Months between departure : " + monthsbetween);
            return monthsbetween;



        }
    }
}
