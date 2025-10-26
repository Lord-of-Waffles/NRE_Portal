// Models/ChartViewModel.cs
namespace MVC_NRE_Portal.Models
{

    // this class contains the data needed to render a chart
    // idk if this will work for charts that aren't bar charts, maybe just needs a small refactor - ben
    public class ChartViewModel
    {
        public List<string> Labels { get; set; }
        public List<double> Data { get; set; }
        public string ChartTitle { get; set; }
        public string ChartId { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }

        public ChartViewModel()
        {
            Labels = new List<string>();
            Data = new List<double>();
            ChartId = Guid.NewGuid().ToString();
            BackgroundColor = "rgba(54, 162, 235, 0.6)";
            BorderColor = "rgba(54, 162, 235, 1)";
        }
    }
}