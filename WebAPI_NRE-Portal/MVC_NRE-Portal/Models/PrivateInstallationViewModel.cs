using System.ComponentModel.DataAnnotations;

namespace MVC_NRE_Portal.Models
{
    public class PrivateInstallationViewModel
    {
        // Wizard state
        public int CurrentStep { get; set; } = 1; // 1..4

        // --- Location ---
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Canton")]
        public string? Canton { get; set; } = "VS";

        [Display(Name = "Municipality")]
        public string? Municipality { get; set; }

        [Display(Name = "Latitude (WGS84)")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude (WGS84)")]
        public double? Longitude { get; set; }

        // --- Type of installation ---
        [Display(Name = "Energy type")]
        [Required(ErrorMessage = "Please select an energy type.")]
        public string? EnergyType { get; set; } // PV, Wind, Mini-Hydraulic, Biogas

        [Display(Name = "Integration type")]
        [Required(ErrorMessage = "Please select an integration type.")]

        public string? IntegrationType { get; set; } // Added / Integrated (PV)

        [Display(Name = "PV cell type")]
        public string? PvCellType { get; set; } // Mono / Poly (PV)

        [Display(Name = "Installed capacity [kW]")]
        [Range(0, 1_000_000)]
        public double? InstalledCapacityKW { get; set; }

        // --- Orientation (US-20) ---
        [Display(Name = "Field orientation (azimuth) [°]")]
        [Range(-180, 180, ErrorMessage = "Azimuth must be between -180 and 180.")]
        public int? Azimuth { get; set; }

        [Display(Name = "Slope of the roof [°]")]
        [Range(0, 90, ErrorMessage = "Slope must be between 0 and 90.")]
        public int? RoofSlope { get; set; }

        // --- Area (US-21) ---
        [Display(Name = "Length [m]")]
        [Range(0, 1000)]
        public double? LengthM { get; set; }

        [Display(Name = "Width [m]")]
        [Range(0, 1000)]
        public double? WidthM { get; set; }

        [Display(Name = "Area [m²]")]
        [Range(0, 1_000_000)]
        public double? AreaM2 { get; set; }

        // Simple helper to compute area if missing
        public void EnsureArea()
        {
            if ((!AreaM2.HasValue || AreaM2.Value <= 0) && LengthM.HasValue && WidthM.HasValue)
                AreaM2 = System.Math.Round(LengthM.Value * WidthM.Value, 2);
        }
    }
}
