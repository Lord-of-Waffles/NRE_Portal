namespace DataLayer_NRE_Portal.Models
{
    /// <summary>
    /// Yearly aggregated canton statistics used for dashboards (non-private).
    /// </summary>
    public class ProductionData
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string EnergyType { get; set; } = "";
        public double ProductionKWh { get; set; }
        public string Canton { get; set; } = "VS";
    }
}
