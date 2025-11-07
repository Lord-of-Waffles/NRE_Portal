using Microsoft.AspNetCore.Mvc;
using MVC_NRE_Portal.Models;

namespace MVC_NRE_Portal.Controllers
{
    public class PrivateInstallationController : Controller
    {
        // GET: /PrivateInstallation?step=1
        public IActionResult Index(int step = 1)
        {
            // clamp 1..4
            if (step < 1) step = 1;
            if (step > 4) step = 4;

            var vm = new PrivateInstallationViewModel
            {
                CurrentStep = step
            };

            return View(vm);
        }

        // POST: just to demo navigation – we don’t persist yet
        [HttpPost]
        public IActionResult Navigate(PrivateInstallationViewModel model, string direction)
        {
            var step = model.CurrentStep;

            if (direction == "next")
                step++;
            else if (direction == "prev")
                step--;

            if (step < 1) step = 1;
            if (step > 4) step = 4;

            return RedirectToAction("Index", new { step });
        }
    }
}
