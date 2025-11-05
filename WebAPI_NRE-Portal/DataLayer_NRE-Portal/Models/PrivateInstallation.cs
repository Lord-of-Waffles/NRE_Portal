namespace DataLayer_NRE_Portal.Models
{
    /// <summary>
    /// User-registered rooftop or small installations with PV-specific fields.
    /// </summary>
    public class PrivateInstallation : InstallationBase
    {
        // Form tabs – Type of installation
        public string? IntegrationType { get; set; }  // Added / Integrated
        public string? PvCellType { get; set; }       // Mono / Poly (when EnergyType = PV)

        // Orientation
        public int? Azimuth { get; set; }             // degrees
        public int? RoofSlope { get; set; }           // degrees

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
