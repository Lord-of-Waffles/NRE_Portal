namespace MVC_NRE_Portal.Models
{
    public class PrivateInstallationViewModel
    {
        public int CurrentStep { get; set; } = 1;

        // STEP 1: Location
        public string? LocationName { get; set; }
        public string? Address { get; set; }

        // STEP 2: Type of installation
        public string? RenewableEnergy { get; set; }  // PV, Biogas, Mini-hydro, Wind
        public string? IntegrationType { get; set; }  // Added / Integrated
        public string? PvCellType { get; set; }       // Poly / Mono

        // STEP 3: Orientation
        public int? Azimuth { get; set; }
        public int? RoofSlope { get; set; }

        // STEP 4: Area
        public double? LengthM { get; set; }
        public double? WidthM { get; set; }
        public double? AreaM2 { get; set; }
    }
}
