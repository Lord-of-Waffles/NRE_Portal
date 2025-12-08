namespace MVC_NRE_Portal.Models
{
    public class PrivateInstallationDto
    {
        public string? EnergyType { get; set; }
        public string? IntegrationType { get; set; }
        public string? PvCellType { get; set; }
        public int? Azimuth { get; set; }
        public int? RoofSlope { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? LengthM { get; set; }
        public double? WidthM { get; set; }
        public double? AreaM2 { get; set; }
        public string? LocationText { get; set; }
        public double? EstimatedKWh { get; set; }
        public double? InstalledCapacityKW { get; set; }
    }
}