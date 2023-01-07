using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace VSFlyWebApp.Services
{
    public class VSFlyServices : IVSFlyServices
    {
        private readonly HttpClient _client;
        private readonly string _baseURI;

        public VSFlyServices(HttpClient client)
        {
            _client = client;
            _baseURI = "https://localhost:7178/api/";
        }

        public async Task<IEnumerable<Models.Flight>> GetAvailableFlights()
        {
            var uri = _baseURI + "Bookings/AvailableFlights";

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var flightList = JsonConvert.DeserializeObject<IEnumerable<Models.Flight>>(data);

            return flightList;
        }

        public async Task<double> GetFlightSalePrice(int id)
        {
            var uri = _baseURI + "Bookings/FlightSalePrice/" + id;

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var price = JsonConvert.DeserializeObject<double>(data);

            return price;
        }

        public async Task<double> GetTotalSalePriceOfFlight(int id)
        {
            var uri = _baseURI + "Bookings/TotalSalePriceOfFlight/" + id;

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var price = JsonConvert.DeserializeObject<double>(data);

            return price;
        }

        public async Task<double> GetAverageSalePriceOfAllFlightsInDestinationTo(string destination)
        {
            var uri = _baseURI + "Bookings/AverageSalePriceOfAllFlightsInDestinationTo/" + destination;

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var price = JsonConvert.DeserializeObject<double>(data);

            return price;
        }

        public async Task<IEnumerable<Models.ResumeBookedTicket>> GetResumeBookedTicketsOfDestination(string destination)
        {
            var uri = _baseURI + "Bookings/ResumeBookedTicketsOfDestination/" + destination;

            var responseJSON = await _client.GetAsync(uri);
            responseJSON.EnsureSuccessStatusCode();

            var data = await responseJSON.Content.ReadAsStringAsync();

            var bookedTickets = JsonConvert.DeserializeObject<IEnumerable<Models.ResumeBookedTicket>>(data);

            return bookedTickets;
        }

        public async Task<bool> BookAFlight(int idPassenger, int idFlight)
        {
            // Build the request URI and request body
            var requestUri = _baseURI + "Bookings/ BookAFlight";
            var requestBody = new { idPassenger = idPassenger, idFlight = idFlight };

            // Serialize the request body to a JSON string
            var requestBodyJson = JsonConvert.SerializeObject(requestBody);

            // Create a new HTTP client and POST request message
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUri),
                Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json")
            };

            // Send the request and get the response
            var response = await client.SendAsync(request);

            // If the response is successful, return true
            // Otherwise, return false
            return response.IsSuccessStatusCode;
        }
    }
}
