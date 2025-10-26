using MVC_NRE_Portal.Models;
using System.Text.Json;

namespace MVC_NRE_Portal.Services
{
    public class ProductionServiceMVC : IProductionServiceMVC
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public ProductionServiceMVC(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _baseUrl = configuration["WebAPI:BaseUrl"];
        }

        public async Task<List<ProductionDataDto>> GetFakeYearData()
        {
            var response = await _client.GetAsync(_baseUrl + "/NRE_Portal");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var data = JsonSerializer.Deserialize<List<ProductionDataDto>>(responseBody, options);
            return data;
        }   
    }
}
