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
            // fake data for testing - Ben
            await Task.Delay(100); // simulate API delay
            
            return new List<ProductionDataDto>
            {
                // Solar power data
                new ProductionDataDto { Id = 1, Year = 2024, ProductionKw = 15000, EnergyType = "Solar", Region = "North" },
                new ProductionDataDto { Id = 2, Year = 2024, ProductionKw = 12000, EnergyType = "Solar", Region = "South" },
                new ProductionDataDto { Id = 3, Year = 2024, ProductionKw = 18000, EnergyType = "Solar", Region = "East" },
                new ProductionDataDto { Id = 4, Year = 2024, ProductionKw = 14000, EnergyType = "Solar", Region = "West" },
                
                // Hydro power data
                new ProductionDataDto { Id = 5, Year = 2024, ProductionKw = 30000, EnergyType = "Hydro", Region = "North" },
                new ProductionDataDto { Id = 6, Year = 2024, ProductionKw = 25000, EnergyType = "Hydro", Region = "South" },
                new ProductionDataDto { Id = 7, Year = 2024, ProductionKw = 28000, EnergyType = "Hydro", Region = "East" },
                new ProductionDataDto { Id = 8, Year = 2024, ProductionKw = 22000, EnergyType = "Hydro", Region = "West" },
                
                // Wind power data
                new ProductionDataDto { Id = 9, Year = 2024, ProductionKw = 20000, EnergyType = "Wind", Region = "North" },
                new ProductionDataDto { Id = 10, Year = 2024, ProductionKw = 17000, EnergyType = "Wind", Region = "South" },
                new ProductionDataDto { Id = 11, Year = 2024, ProductionKw = 23000, EnergyType = "Wind", Region = "East" },
                new ProductionDataDto { Id = 12, Year = 2024, ProductionKw = 19000, EnergyType = "Wind", Region = "West" },
                
                // Natural Gas data
                new ProductionDataDto { Id = 17, Year = 2024, ProductionKw = 28000, EnergyType = "Natural Gas", Region = "North" },
                new ProductionDataDto { Id = 18, Year = 2024, ProductionKw = 26000, EnergyType = "Natural Gas", Region = "South" },
                new ProductionDataDto { Id = 19, Year = 2024, ProductionKw = 24000, EnergyType = "Natural Gas", Region = "East" },
                new ProductionDataDto { Id = 20, Year = 2024, ProductionKw = 29000, EnergyType = "Natural Gas", Region = "West" }
            };

            /* Hugo's old code:
            
            var response = await _client.GetAsync(_baseUrl + "/NRE_Portal");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var data = JsonSerializer.Deserialize<List<ProductionDataDto>>(responseBody, options);
            return data;
            
            */
        }   
    }
}