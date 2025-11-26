using DataLayer_NRE_Portal;
using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_NRE_Portal.Services
{
    public class ProductionService : IProductionService
    {
        private readonly NrePortalContext _context;
        public ProductionService(NrePortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductionData>> GetProductionData(string canton)
        {
            return await _context.ProductionSummaries.Where(x => x.Canton == canton)
                .OrderBy(x => x.Year)
                .ToListAsync();
        }

        public List<ProductionDataDto> GetFakeYearProduction()
        {
            
            var exampleData = new List<ProductionDataDto>
            {
                new ProductionDataDto { Year = 2015, ProductionKw = 13250, EnergyType = "Solar", Region = "VS" },
                new ProductionDataDto { Year = 2016, ProductionKw = 14020, EnergyType = "Wind", Region = "VS" },
                new ProductionDataDto { Year = 2017, ProductionKw = 15870, EnergyType = "Hydro", Region = "VS" },
                new ProductionDataDto { Year = 2018, ProductionKw = 16750, EnergyType = "Solar", Region = "VS" },
                new ProductionDataDto { Year = 2019, ProductionKw = 17600, EnergyType = "Wind", Region = "VS" },
                new ProductionDataDto { Year = 2020, ProductionKw = 18900, EnergyType = "Hydro", Region = "VS" },
                new ProductionDataDto { Year = 2021, ProductionKw = 19200, EnergyType = "Solar", Region = "VS" },
                new ProductionDataDto { Year = 2022, ProductionKw = 20500, EnergyType = "Wind", Region = "VS" },
                new ProductionDataDto { Year = 2023, ProductionKw = 21700, EnergyType = "Hydro", Region = "VS" },
                new ProductionDataDto { Year = 2024, ProductionKw = 22800, EnergyType = "Solar", Region = "VS" }
            };
            return exampleData;
        }
    }
    public class ProductionDataDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public double ProductionKw { get; set; }
        public string EnergyType { get; set; }
        public string Region { get; set; }
    }
}
