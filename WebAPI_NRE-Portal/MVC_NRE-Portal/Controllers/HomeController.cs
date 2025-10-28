using Microsoft.AspNetCore.Mvc;
using MVC_NRE_Portal.Models;
using MVC_NRE_Portal.Services;
using System.Diagnostics;

namespace MVC_NRE_Portal.Controllers
{
    public class HomeController : Controller
    {
        private IProductionServiceMVC _productionServiceMVC;

        public HomeController(IProductionServiceMVC productionServiceMVC)
        {
            _productionServiceMVC = productionServiceMVC;
        }

        public async Task<IActionResult> Index()
        {
            var yearKwProduction = await _productionServiceMVC.GetFakeYearData();
            return View(yearKwProduction);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
