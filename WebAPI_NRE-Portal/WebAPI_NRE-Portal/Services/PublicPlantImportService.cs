using System.Globalization;
using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Geo;
using DataLayer_NRE_Portal.Models;

namespace WebAPI_NRE_Portal.Services
{
    public class PublicPlantImportService
    {
        private readonly NrePortalContext _ctx;

        public PublicPlantImportService(NrePortalContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Import a simple CSV with columns like:
        /// ID;NAME;TECHNOLOGY;POWER_KW;CANTON;MUNICIPALITY;X_LV95;Y_LV95
        /// Delimiter can be ; or , depending on your file. Numbers use '.' decimal.
        /// </summary>
        public int ImportCsv(string path, string cantonFilter = "VS", char delimiter = ';')
        {
            int inserted = 0;
            foreach (var line in File.ReadLines(path).Skip(1))
            {
                var parts = line.Split(delimiter);
                if (parts.Length < 8) continue;

                string canton = parts[4].Trim();
                if (!string.Equals(canton, cantonFilter, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Parse numbers culture-invariant
                double powerKw = ParseDouble(parts[3]);
                double? x = ParseNullable(parts[6]);
                double? y = ParseNullable(parts[7]);

                double? lat = null;
                double? lon = null;
                if (x.HasValue && y.HasValue)
                {
                    var (la, lo) = GeoCoordConverter.Lv95ToWgs84(x.Value, y.Value);
                    lat = la;
                    lon = lo;
                }

                var plant = new PublicInstallation
                {
                    Name = parts[1].Trim(),
                    EnergyType = parts[2].Trim(),          // map to PV/Wind/Hydro/Biogas if needed
                    Region = canton,
                    InstalledCapacityKW = powerKw,
                    Municipality = parts[5].Trim(),
                    SourceX = x,
                    SourceY = y,
                    SourceCrs = "EPSG:2056",
                    Latitude = lat,
                    Longitude = lon,
                    SourceRef = parts[0].Trim()
                };

                _ctx.PublicInstallations.Add(plant);
                inserted++;
            }

            _ctx.SaveChanges();
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
