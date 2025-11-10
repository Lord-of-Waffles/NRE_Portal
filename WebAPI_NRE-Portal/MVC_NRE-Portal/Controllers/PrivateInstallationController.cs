using Microsoft.AspNetCore.Mvc;
using MVC_NRE_Portal.Models;

namespace MVC_NRE_Portal.Controllers
{
    public class PrivateInstallationController : Controller
    {
        // GET /PrivateInstallation?step=1
        [HttpGet]
        public IActionResult Index(int step = 1)
        {
            var model = new PrivateInstallationViewModel { CurrentStep = Normalize(step) };
            return View(model);
        }

        // POST /PrivateInstallation/Navigate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Navigate(PrivateInstallationViewModel model, string direction)
        {
            // keep computed values in sync
            model.EnsureArea();

            var step = Normalize(model.CurrentStep);

            if (string.Equals(direction, "next", System.StringComparison.OrdinalIgnoreCase))
                step++;
            else if (string.Equals(direction, "prev", System.StringComparison.OrdinalIgnoreCase))
                step--;

            model.CurrentStep = Normalize(step);
            ModelState.Clear(); // we show light validation per step; full validation can come when saving
            return View("Index", model);
        }

        // Optional: direct tab navigation /PrivateInstallation/Go?step=3
        [HttpGet]
        public IActionResult Go(int step, [FromQuery] PrivateInstallationViewModel carry)
        {
            carry.CurrentStep = Normalize(step);
            carry.EnsureArea();
            ModelState.Clear();
            return View("Index", carry);
        }

        // Later, hook to API: POST to /api/privateinstallations
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(PrivateInstallationViewModel model)
        {
            model.EnsureArea();

            // TODO: Call WebAPI when ready
            // using HttpClient to POST model as JSON to /api/privateinstallations

            TempData["Saved"] = "Private installation saved (stub).";
            return RedirectToAction(nameof(Index), new { step = 1 });
        }

        private static int Normalize(int s) => s < 1 ? 1 : (s > 4 ? 4 : s);
    }
}
