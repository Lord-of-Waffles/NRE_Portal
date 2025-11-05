namespace DataLayer_NRE_Portal.Models
{
    /// <summary>
    /// Common fields shared by all installations (public & private).
    /// </summary>
    public abstract class InstallationBase
    {
        public int Id { get; set; }

        // Identity / classification
        public string Name { get; set; } = "";
        public string EnergyType { get; set; } = "";   
        public string Region { get; set; } = "VS";    

        // Technical
        public double InstalledCapacityKW { get; set; }      // Nominal capacity
        public double? AnnualProductionKWh { get; set; }     

        // Geo
        public double? Latitude { get; set; }                // WGS84 latitude (decimal degrees)
        public double? Longitude { get; set; }               // WGS84 longitude (decimal degrees)

        // Original CRS (for Swiss datasets in LV95 EPSG:2056)
        public double? SourceX { get; set; }                 // easting (meters)
        public double? SourceY { get; set; }                 // northing (meters)
        public string? SourceCrs { get; set; }               // e.g., "EPSG:2056"

        // Timeline
        public DateTime? CommissioningDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
