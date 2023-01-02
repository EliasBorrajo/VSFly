using Newtonsoft.Json;

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
    }
}
