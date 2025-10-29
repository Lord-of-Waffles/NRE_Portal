namespace MVC_NRE_Portal.Models
{
    public class ProductionDataDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public double ProductionKw { get; set; }

        // provide default values to satisfy non-nullable reference types
        public string EnergyType { get; set; } = string.Empty;
        public string Region { get; set; } = "VS";
    }
}
