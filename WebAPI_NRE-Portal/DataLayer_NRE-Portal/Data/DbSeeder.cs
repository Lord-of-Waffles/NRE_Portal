using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Geo;
using DataLayer_NRE_Portal.Models;
using System.Globalization;
using System.Resources;

namespace WebAPI_NRE_Portal.Data
{
    public static class DbSeeder
    {
        public static void Seed(NrePortalContext ctx)
        {
            // Path to CSV files
            string solutionRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;
            string basePath = Path.Combine(solutionRoot, "DataLayer_NRE-Portal", "Resources");

            // Production summaries
            if (!ctx.ProductionSummaries.Any())
            {
                string file = Path.Combine(basePath, "ProductionSummaries.csv");
                int count = ImportProductionSummaries(ctx, file);
                Console.WriteLine($"✅ {count} ProductionSummaries importées.");
            }   

            // Public installations
            if (!ctx.PublicInstallations.Any())
            {
                string file = Path.Combine(basePath, "ElectricityProductionPlant.csv");
                int count = ImportPublicInstallations(ctx, file);
                Console.WriteLine($"✅ {count} PublicInstallations imported.");
            }

            // Code of Dehlya
            /* // Production summaries (tiny sample; extend later)
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
             }   */

            ctx.SaveChanges();
        }

        private static int ImportProductionSummaries(NrePortalContext ctx, string path, char delimiter = ',')
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"⚠️ Fichier introuvable : {path}");
                return 0;
            }

            int inserted = 0;
            foreach (var line in File.ReadLines(path).Skip(1))
            {

                var parts = line.Split(delimiter);

                var data = new ProductionData
                {
                    Year = int.Parse(parts[0].Trim()),
                    ProductionKWh = ParseDouble(parts[1]),
                    EnergyType = parts[2].Trim(),
                    Canton = "VS"
                };

                ctx.ProductionSummaries.Add(data);
                inserted++;
            }

            return inserted;
        }

        private static int ImportPublicInstallations(NrePortalContext ctx, string path, string cantonFilter = "VS", char delimiter = ',')
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"⚠️ Fichier introuvable : {path}");
                return 0;
            }

            var CategoryMap = new Dictionary<string, string>
            {
                { "subcat_1", "Hydroelectric power" },
                { "subcat_2", "Photovoltaic" },
                { "subcat_3", "Wind energy" },
                { "subcat_4", "Biomass" },
                { "subcat_5", "Geothermal energy" },
                { "subcat_6", "Nuclear energy" },
                { "subcat_7", "Crude oil" },
                { "subcat_8", "Natural gas" },
                { "subcat_9", "Coal" },
                { "subcat_10", "Waste" },
            };

            int inserted = 0;
            foreach (var line in File.ReadLines(path))
            {
                var parts = line.Split(delimiter);

                string canton = parts[4].Trim();
                if (!string.Equals(canton, cantonFilter, StringComparison.OrdinalIgnoreCase))
                    continue;

                double powerKw = ParseDouble(parts[7]);
                double? x = ParseNullable(parts[11]);
                double? y = ParseNullable(parts[12]);

                double? lat = null, lon = null;
                if (x.HasValue && y.HasValue)
                {
                    var (la, lo) = GeoCoordConverter.Lv95ToWgs84(x.Value, y.Value);
                    lat = la;
                    lon = lo;
                }

                string CategoryId = parts.Length > 8 ? parts[9].Trim() : null;
                string CategoryName = (CategoryId != null && CategoryMap.ContainsKey(CategoryId))
                                          ? CategoryMap[CategoryId]
                                          : "Unknown";

                var plant = new PublicInstallation
                {
                    EnergyType = CategoryName,
                    Region = canton,
                    InstalledCapacityKW = powerKw,
                    Municipality = parts[3].Trim(),
                    SourceX = x,
                    SourceY = y,
                    SourceCrs = "EPSG:2056",
                    Latitude = lat,
                    Longitude = lon,
                    SourceRef = parts[0].Trim()
                };
                ctx.PublicInstallations.Add(plant);
                inserted++;
            }

            return inserted;
        }

        private static double ParseDouble(string s) =>
            double.Parse(s.Trim().Replace(',', '.'), CultureInfo.InvariantCulture);

        private static double? ParseNullable(string s)
        {
            s = s.Trim();
            if (string.IsNullOrWhiteSpace(s)) return null;
            return double.Parse(s.Replace(',', '.'), CultureInfo.InvariantCulture);
        }
    }
}
