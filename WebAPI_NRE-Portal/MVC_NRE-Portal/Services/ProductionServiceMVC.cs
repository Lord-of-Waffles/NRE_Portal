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

        public Task<List<ProductionDataDto>> GetFakeYearData()
        {
            var rows = BuildYearlyProductionFromTable_KWh_2010_2018();
            return Task.FromResult(rows);
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

        /// <summary>
        /// Exact figures from the provided Valais table (image) for years 2010–2018.
        /// Table units are GWh → we store kWh (1 GWh = 1,000,000 kWh).
        /// Energy types: PV, Mini-Hydraulic, Windturbine, Biogas.
        /// </summary>
        private static List<ProductionDataDto> BuildYearlyProductionFromTable_KWh_2010_2018()
        {
            const double KWH_PER_GWH = 1_000_000d;

            var rows = new List<ProductionDataDto>();

            // ----- Mini-Hydraulic -----
            var miniHydroGWh = new (int Year, double GWh)[]
            {
                (2010, 429),
                (2011, 401),
                (2012, 451),
                (2013, 435),
                (2014, 492),
                (2015, 496),
                (2016, 512),
                (2017, 474),
                (2018, 549)
            };

            // ----- Biogas -----
            var biogasGWh = new (int Year, double GWh)[]
            {
                (2010, 3),
                (2011, 3),
                (2012, 4),
                (2013, 4),
                (2014, 4),
                (2015, 5),
                (2016, 5),
                (2017, 5),
                (2018, 5)
            };

            // ----- PV -----
            var pvGWh = new (int Year, double GWh)[]
            {
                (2010, 1),
                (2011, 1),
                (2012, 6),
                (2013, 22),
                (2014, 41),
                (2015, 60),
                (2016, 67),
                (2017, 74),
                (2018, 84)
            };

            // ----- Windturbine -----
            var windGWh = new (int Year, double GWh)[]
            {
                (2010, 10),
                (2011, 9),
                (2012, 14),
                (2013, 19),
                (2014, 18),
                (2015, 18),
                (2016, 18),
                (2017, 24),
                (2018, 22)
            };

            rows.AddRange(miniHydroGWh.Select(x => new ProductionDataDto
            {
                Year = x.Year,
                ProductionKw = x.GWh * KWH_PER_GWH,
                EnergyType = "Hydroelectric power", // ← CHANGE THIS LINE
                Region = "VS"
            }));

            rows.AddRange(biogasGWh.Select(x => new ProductionDataDto
            {
                Year = x.Year,
                ProductionKw = x.GWh * KWH_PER_GWH,
                EnergyType = "Biogas", // ← KEEP THIS
                Region = "VS"
            }));

            rows.AddRange(pvGWh.Select(x => new ProductionDataDto
            {
                Year = x.Year,
                ProductionKw = x.GWh * KWH_PER_GWH,
                EnergyType = "Photovoltaic", // ← CHANGE THIS LINE
                Region = "VS"
            }));

            rows.AddRange(windGWh.Select(x => new ProductionDataDto
            {
                Year = x.Year,
                ProductionKw = x.GWh * KWH_PER_GWH,
                EnergyType = "Wind energy", // ← CHANGE THIS LINE
                Region = "VS"
            }));

            return rows
                .OrderBy(r => r.Year)
                .ThenBy(r => r.EnergyType)
                .ToList();
        }


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
