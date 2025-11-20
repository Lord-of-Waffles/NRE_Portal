using Microsoft.AspNetCore.Mvc;
using MVC_NRE_Portal.Models;
using MVC_NRE_Portal.Services;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            var data = await _productionService.GetProductionSummary();

            var grouped = data
                .GroupBy(d => d.EnergyType)
                .Select(g => new { Type = g.Key, Total = g.Sum(x => x.ProductionKw) })
                .OrderByDescending(x => x.Total)
                .ToList();

            var vm = new ChartViewModel
            {
                Labels = grouped.Select(x => x.Type).ToList(),
                Data = grouped.Select(x => x.Total).ToList(),
                ChartTitle = "Total Production by Energy Type (kWh)",
                BackgroundColor = "rgba(75, 192, 192, 0.6)",
                BorderColor = "rgba(75, 192, 192, 1)",
                ChartId = "energyDashboardChart"
            };

            return View(vm);
        }   

        // Ancient code
     /*   // Dashboard: bar chart comparing total production by energy type (kWh)
        public async Task<IActionResult> Dashboard()
        {
            var data = await _productionService.GetFakeYearData();

            var grouped = data
                .GroupBy(d => d.EnergyType)
                .Select(g => new { Type = g.Key, Total = g.Sum(x => x.ProductionKw) })
                .OrderByDescending(x => x.Total)
                .ToList();

            var vm = new ChartViewModel
            {
                Labels = grouped.Select(x => x.Type).ToList(),
                Data = grouped.Select(x => x.Total).ToList(),
                ChartTitle = "Total Production by Energy Type (kWh)",
                BackgroundColor = "rgba(75, 192, 192, 0.6)",
                BorderColor = "rgba(75, 192, 192, 1)",
                ChartId = "energyDashboardChart"
            };

            return View(vm);
        }   */

        // One page per energy type (kWh)
        public async Task<IActionResult> PV() => await Energy("Photovoltaic", "Photovoltaic (PV) Production (kWh)");
        public async Task<IActionResult> MiniHydro() => await Energy("Hydroelectric power", "Mini-Hydraulic Production (kWh)");
        public async Task<IActionResult> Wind() => await Energy("Wind energy", "Windturbine Production (kWh)");
        public async Task<IActionResult> Biogas() => await Energy("Biogas", "Biogas Production (kWh)");

        private async Task<IActionResult> Energy(string energyType, string title)
        {
            var data = await _productionService.GetProductionSummary();

            var series = data
                .Where(d => d.EnergyType == energyType)
                .OrderBy(d => d.Year)
                .ToList();

            var vm = new ChartViewModel
            {
                Labels = series.Select(x => x.Year.ToString()).ToList(),
                Data = series.Select(x => x.ProductionKw).ToList(),
                ChartTitle = title,
                BackgroundColor = "rgba(54, 162, 235, 0.5)",
                BorderColor = "rgba(54, 162, 235, 1)",
                ChartId = $"energyPageChart_{energyType.Replace("-", "").Replace(" ", "")}"
            };

            return View("EnergyPage", vm);
        }
    }
}
