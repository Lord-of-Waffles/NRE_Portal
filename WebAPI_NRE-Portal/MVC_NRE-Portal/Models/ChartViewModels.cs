using System;
using System.Collections.Generic;

namespace MVC_NRE_Portal.Models
{
    // this class contains the data needed to render a chart
    public class ChartViewModel
    {
        public List<string> Labels { get; set; } = new();
        public List<double> Data { get; set; } = new();
        public string ChartTitle { get; set; } = string.Empty;

        // kept for compatibility with views/controllers using `Id`
        public string Id { get; set; }

        // provide `ChartId` as an alias so existing views/components referencing ChartId keep working
        public string ChartId
        {
            get => Id;
            set => Id = value;
        }

        public string BackgroundColor { get; set; } = "rgba(54, 162, 235, 0.6)";
        public string BorderColor { get; set; } = "rgba(54, 162, 235, 1)";

        public ChartViewModel()
        {
            // GUID without hyphens and prefixed to be safe as an HTML id
            Id = "chart_" + Guid.NewGuid().ToString("N");
        }
    }
}