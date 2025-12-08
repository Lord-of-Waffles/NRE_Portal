using System.Collections.Generic;
using System.Linq;
using System.Net.Http; // typed HttpClient
using System.Text.Json;
using System.Threading.Tasks;
using MVC_NRE_Portal.Models;

namespace MVC_NRE_Portal.Services
{
    public class ProductionServiceMVC : IProductionServiceMVC
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public ProductionServiceMVC(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _baseUrl = configuration["WebAPI:BaseUrl"];
        }

        public async Task<List<ProductionDataDto>> GetProductionSummary()
        {
            var response = await _http.GetAsync(_baseUrl + "/ProductionSummaries");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var productionSummaries = JsonSerializer.Deserialize<List<ProductionDataDto>>(responseBody, options);
            return productionSummaries;
        }
    }
}
