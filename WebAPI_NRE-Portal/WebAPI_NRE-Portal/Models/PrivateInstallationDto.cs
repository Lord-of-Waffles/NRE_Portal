namespace WebAPI_NRE_Portal.Models
{
    public class PrivateInstallationDto
    {
        public int Id { get; set; }
        
        // FROM INSTALLATIONBASE - ADD THESE
        public string Name { get; set; } = "";
        public string EnergyType { get; set; } = "";
        public string Region { get; set; } = "VS";
        public double InstalledCapacityKW { get; set; }
        public double? AnnualProductionKWh { get; set; }
        public DateTime? CommissioningDate { get; set; }  // need to have this for 2025 data - Ben
        
        // Form tabs – Type of installation
        public string? IntegrationType { get; set; }  // Added / Integrated
        public string? PvCellType { get; set; }       // Mono / Poly (when EnergyType = PV)

        // Orientation
        public int? Azimuth { get; set; }             // degrees
        public int? RoofSlope { get; set; }           // degrees

        // Geo
        public double? Latitude { get; set; }                // WGS84 latitude (decimal degrees)
        public double? Longitude { get; set; }               // WGS84 longitude (decimal degrees)

        // Area
        public double? LengthM { get; set; }
        public double? WidthM { get; set; }
        public double? AreaM2 { get; set; }

        // Computed estimate (kWh/year)
        public double? EstimatedKWh { get; set; }

        // Simple free-text location (street, etc.)
        public string? LocationText { get; set; }
    }
}