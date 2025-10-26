// Controllers/ProductionController.cs
using Microsoft.AspNetCore.Mvc;
using MVC_NRE_Portal.Models;
using MVC_NRE_Portal.Services;

namespace MVC_NRE_Portal.Controllers
{
    public class ProductionController : Controller
    {
        private readonly IProductionServiceMVC _productionService;

        public ProductionController(IProductionServiceMVC productionService)
        {
            _productionService = productionService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var productionData = await _productionService.GetFakeYearData();

            // Group by energy type and sum production
            var groupedData = productionData
                .GroupBy(p => p.EnergyType)
                .Select(g => new
                {
                    EnergyType = g.Key,
                    TotalProduction = g.Sum(p => p.ProductionKw)
                })
                .OrderByDescending(x => x.TotalProduction)
                .ToList();

            // Create chart view model
            var chartData = new ChartViewModel
            {
                Labels = groupedData.Select(d => d.EnergyType).ToList(),
                Data = groupedData.Select(d => d.TotalProduction).ToList(),
                ChartTitle = "Production by Energy Type",
                BackgroundColor = "rgba(75, 192, 192, 0.6)",
                BorderColor = "rgba(75, 192, 192, 1)"
            };

            return View(chartData);
        }
    }
}