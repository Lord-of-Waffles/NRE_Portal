namespace MVC_NRE_Portal.Models
{
    public class ProductionDataDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public double ProductionKw { get; set; }
        public string EnergyType { get; set; }
        public string Region { get; set; }
    }
}
