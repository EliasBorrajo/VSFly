using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace VSFlyWebApp.Services
{
    public class VSFlyServices : IVSFlyServices
    {
        private readonly HttpClient _client;
        private readonly string _baseURI;

        /// <summary>
        /// Initializes a new instance of the VSFlyServices class.
        /// </summary>
        /// <param name="client">The HTTP client used for making requests to the API.</param>
        public VSFlyServices(HttpClient client)
        {
            _client = client;
            _baseURI = "https://localhost:7178/api/";
        }

        /// <summary>
        /// Gets a list of available flights from the API.
        /// </summary>
        /// <returns>A list of available flights.</returns>
        public async Task<IEnumerable<Models.Flight>> GetAvailableFlights()
        {
            var uri = _baseURI + "Bookings/AvailableFlights";

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var flightList = JsonConvert.DeserializeObject<IEnumerable<Models.Flight>>(data);

            return flightList;
        }

        /// <summary>
        /// Gets the sale price of a specific flight from the API.
        /// </summary>
        /// <param name="id">The id of the flight to get the sale price for.</param>
        /// <returns>The sale price of the specified flight.</returns>
        public async Task<double> GetFlightSalePrice(int id)
        {
            var uri = _baseURI + "Bookings/FlightSalePrice/" + id;

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var price = JsonConvert.DeserializeObject<double>(data);

            return price;
        }

        /// <summary>
        /// Gets the total sale price of a specific flight from the API.
        /// </summary>
        /// <param name="id">The id of the flight to get the total sale price for.</param>
        /// <returns>The total sale price of the specified flight.</returns>
        public async Task<double> GetTotalSalePriceOfFlight(int id)
        {
            var uri = _baseURI + "Bookings/TotalSalePriceOfFlight/" + id;

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var price = JsonConvert.DeserializeObject<double>(data);

            return price;
        }

        /// <summary>
        /// Gets the average sale price of all flights to a specific destination from the API.
        /// </summary>
        /// <param name="destination">The destination to get the average sale price for.</param>
        /// <returns>The average sale price of all flights to the specified destination.</returns>
        public async Task<double> GetAverageSalePriceOfAllFlightsInDestinationTo(string destination)
        {
            var uri = _baseURI + "Bookings/AverageSalePriceOfAllFlightsInDestinationTo/" + destination;

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var price = JsonConvert.DeserializeObject<double>(data);

            return price;
        }

        /// <summary>
        /// Gets a list of booked tickets for flights to a specific destination from the API.
        /// </summary>
        /// <param name="destination">The destination to get the booked tickets for.</param>
        /// <returns>A list of booked tickets for flights to the specified destination.</returns>
        public async Task<IEnumerable<Models.ResumeBookedTicket>> GetResumeBookedTicketsOfDestination(string destination)
        {
            var uri = _baseURI + "Bookings/ResumeBookedTicketsOfDestination/" + destination;

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var bookedTickets = JsonConvert.DeserializeObject<IEnumerable<Models.ResumeBookedTicket>>(data);

            return bookedTickets;
        }

        /// <summary>
        /// Books a flight with the specified passenger and flight id using the API.
        /// </summary>
        /// <param name="idPassenger">The id of the passenger booking the flight.</param>
        /// <param name="idFlight">The id of the flight being booked.</param>
        /// <returns>True if the flight was successfully booked, false otherwise.</returns>
        public async Task<bool> BookAFlight(int idPassenger, int idFlight)
        {
            // Build the request URI and request body
            var requestUri = _baseURI + "Bookings/BookAFlight/" + idPassenger + "/" + idFlight;

            // Serialize the request body to a JSON string
            var requestBodyJson = JsonConvert.SerializeObject(requestUri);

            // Create a new HTTP client and POST request message
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUri),
                Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json")
            };

            Console.WriteLine("Request: " + request);

            // Send the request and get the response
            var response = await client.SendAsync(request);

            // If the response is successful, return true
            // Otherwise, return false
            return response.IsSuccessStatusCode;
        }
    }
}
