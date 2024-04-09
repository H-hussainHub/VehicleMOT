using Newtonsoft.Json;

namespace VehicleMOTInfo.Data
{
    public class MOTService
    {
        private readonly HttpClient _httpClient;

        public MOTService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VehicleInfo>?> GetVehicleInfo(string registrationNumber)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-api-key", "fZi8YcjrZN1cGkQeZP7Uaa4rTxua8HovaswPuIno");

                var response = await _httpClient.GetAsync($"https://beta.check-mot.service.gov.uk/trade/vehicles/mot-tests?registration={registrationNumber}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}. {response.ReasonPhrase}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
   
                var vehicleInfos = JsonConvert.DeserializeObject<List<VehicleInfo>>(content);
                return vehicleInfos;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HttpRequestException: {e.Message}");
                return null;
            }
            catch (JsonSerializationException e)
            {
                Console.WriteLine($"JsonSerializationException: {e.Message}");
                return null;
            }
        }



    }
}
