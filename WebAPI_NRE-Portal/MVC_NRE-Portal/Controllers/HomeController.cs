using Microsoft.AspNetCore.Mvc;
using MVC_NRE_Portal.Models;
using MVC_NRE_Portal.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_NRE_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductionServiceMVC _productionServiceMVC;

        public HomeController(IProductionServiceMVC productionServiceMVC)
        {
            _productionServiceMVC = productionServiceMVC;
        }

        public async Task<IActionResult> Index()
        {
            // Pull all rows (PV, Mini-Hydraulic, Windturbine, Biogas)
            var all = await _productionServiceMVC.GetProductionSummary();

            // Aggregate to ONE value per year so the home chart is smooth, not zig-zag
            var yearlyTotals = all
                .GroupBy(d => d.Year)
                .Select(g => new { Year = g.Key, Total = g.Sum(x => x.ProductionKw) })
                .OrderBy(x => x.Year)
                .ToList();

            var vm = new ChartViewModel
            {
                Labels = yearlyTotals.Select(x => x.Year.ToString()).ToList(),
                Data = yearlyTotals.Select(x => x.Total).ToList(),
                ChartTitle = "Total Renewable Energy Production in Valais (GWh)",
                BackgroundColor = "rgba(75, 192, 192, 0.4)",
                BorderColor = "rgba(75, 192, 192, 1)",
                ChartId = "homeTotalChart"
            };

            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}