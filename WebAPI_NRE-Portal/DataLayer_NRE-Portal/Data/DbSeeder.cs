using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;

namespace WebAPI_NRE_Portal.Data
{
    public static class DbSeeder
    {
        public static void Seed(NrePortalContext ctx)
        {
            // Production summaries (tiny sample; extend later)
            if (!ctx.ProductionSummaries.Any())
            {
                ctx.ProductionSummaries.AddRange(
                    new ProductionData { Year = 2010, EnergyType = "PV", ProductionKWh = 1_000_000, Canton = "VS" },
                    new ProductionData { Year = 2010, EnergyType = "Windturbine", ProductionKWh = 10_000_000, Canton = "VS" },
                    new ProductionData { Year = 2010, EnergyType = "Biogas", ProductionKWh = 3_000_000, Canton = "VS" },
                    new ProductionData { Year = 2010, EnergyType = "Mini-Hydraulic", ProductionKWh = 429_000_000, Canton = "VS" }
                );
            }

            // One public plant (fake)
            if (!ctx.PublicInstallations.Any())
            {
                ctx.PublicInstallations.Add(new PublicInstallation
                {
                    Name = "VS Demo Hydro",
                    EnergyType = "Hydro",
                    Region = "VS",
                    InstalledCapacityKW = 50000,
                    AnnualProductionKWh = 120_000_000,
                    Municipality = "Sion",
                    Latitude = 46.233,
                    Longitude = 7.360,
                    SourceCrs = "EPSG:4326",
                    IsActive = true
                });
            }

            // One private install (fake)
            if (!ctx.PrivateInstallations.Any())
            {
                ctx.PrivateInstallations.Add(new PrivateInstallation
                {
                    Name = "Rooftop PV #1",
                    EnergyType = "PV",
                    Region = "VS",
                    InstalledCapacityKW = 6.0,
                    PvCellType = "Monocrystalline",
                    IntegrationType = "Integrated",
                    Azimuth = 0,
                    RoofSlope = 25,
                    LengthM = 10,
                    WidthM = 4,
                    AreaM2 = 40,
                    EstimatedKWh = 40 * 250 * 1.0, // 10,000 kWh/y (your PV rule of thumb)
                    LocationText = "Sion",
                    Latitude = 46.238,
                    Longitude = 7.358
                });
            }

            ctx.SaveChanges();
        }
    }
}
