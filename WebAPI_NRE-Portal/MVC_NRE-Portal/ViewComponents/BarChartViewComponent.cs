// ViewComponents/BarChartViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using MVC_NRE_Portal.Models;

namespace MVC_NRE_Portal.ViewComponents
{

    // component for displaying bar charts, reusable incase we neeed it for other stuff - ben
    public class BarChartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ChartViewModel model)
        {
            if (model == null)
            {
                model = new ChartViewModel
                {
                    ChartTitle = "No Data Available"
                };
            }

            return View(model);
        }
    }
}