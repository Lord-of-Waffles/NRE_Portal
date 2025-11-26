using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Geo;
using DataLayer_NRE_Portal.Models;
using System.Globalization;

namespace WebAPI_NRE_Portal.Data
{
    public static class DbSeeder
    {
        public static void Seed(NrePortalContext ctx)
        {
            // Try container path first, fallback to development path
            string basePath;
            
            // In container, CSVs should be at /app/Resources
            string containerPath = Path.Combine(AppContext.BaseDirectory, "Resources");
            
            if (Directory.Exists(containerPath))
            {
                basePath = containerPath;
                Console.WriteLine($" Using container path: {basePath}");
            }
            else
            {
                // Development: navigate up from bin folder
                try
                {
                    string? solutionRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
                    if (solutionRoot == null)
                    {
                        Console.WriteLine("️ Could not find solution root. Skipping seeding.");
                        return;
                    }
                    basePath = Path.Combine(solutionRoot, "DataLayer_NRE-Portal", "Resources");
                    Console.WriteLine($" Using development path: {basePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"️ Error finding resources path: {ex.Message}. Skipping seeding.");
                    return;
                }
            }
            
            if (!Directory.Exists(basePath))
            {
                Console.WriteLine($" Resources directory not found at {basePath}. Skipping seeding.");
                return;
            }

            // Production summaries
            if (!ctx.ProductionSummaries.Any())
            {
                string file = Path.Combine(basePath, "ProductionSummaries.csv");
                if (File.Exists(file))
                {
                    int count = ImportProductionSummaries(ctx, file);
                    Console.WriteLine($" {count} ProductionSummaries imported.");
                }
                else
                {
                    Console.WriteLine($"️ ProductionSummaries.csv not found.");
                }
            }   

            // Public installations
            if (!ctx.PublicInstallations.Any())
            {
                string file = Path.Combine(basePath, "ElectricityProductionPlant.csv");
                if (File.Exists(file))
                {
                    int count = ImportPublicInstallations(ctx, file);
                    Console.WriteLine($" {count} PublicInstallations imported.");
                }
                else
                {
                    Console.WriteLine($"️ ElectricityProductionPlant.csv not found.");
                }
            }

            ctx.SaveChanges();
        }

        private static int ImportProductionSummaries(NrePortalContext ctx, string path, char delimiter = ',')
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"️ Fichier introuvable : {path}");
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
                Console.WriteLine($" Fichier introuvable : {path}");
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