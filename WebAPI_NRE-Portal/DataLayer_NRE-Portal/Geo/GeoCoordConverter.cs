using System;

namespace DataLayer_NRE_Portal.Geo
{
    /// <summary>
    /// Minimal converters for Swiss coordinates (LV95 / EPSG:2056) to WGS84 lat/lon.
    /// Good enough for map display. Input in meters, output degrees.
    /// References: swisstopo public formulas.
    /// </summary>
    public static class GeoCoordConverter
    {
        // Convert LV95 (E,N meters; E around 2_600_000, N around 1_200_000) to WGS84 (lat, lon).
        public static (double lat, double lon) Lv95ToWgs84(double e, double n)
        {
            // Normalize
            double e1 = (e - 2600000.0) / 1000000.0;
            double n1 = (n - 1200000.0) / 1000000.0;

            // Longitude (lambda), Latitude (phi) in seconds of arc
            double lonSec =
                2.6779094 +
                4.728982 * e1 +
                0.791484 * e1 * n1 +
                0.1306 * e1 * Math.Pow(n1, 2) -
                0.0436 * Math.Pow(e1, 3);

            double latSec =
                16.9023892 +
                3.238272 * n1 -
                0.270978 * Math.Pow(e1, 2) -
                0.002528 * Math.Pow(n1, 2) -
                0.0447 * Math.Pow(e1, 2) * n1 -
                0.0140 * Math.Pow(n1, 3);

            // Convert seconds to degrees
            double lat = latSec * (1.0 / 3600.0);
            double lon = lonSec * (1.0 / 3600.0);
            return (lat, lon);
        }

        // Convenience method that accepts nullable inputs
        public static (double? lat, double? lon) TryLv95ToWgs84(double? e, double? n)
        {
            if (e.HasValue && n.HasValue) return Lv95ToWgs84(e.Value, n.Value);
            return (null, null);
        }
    }
}
