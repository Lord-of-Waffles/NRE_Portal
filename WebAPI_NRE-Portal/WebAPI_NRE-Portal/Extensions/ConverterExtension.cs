namespace WebAPI_NRE_Portal.Extensions
{
    public static class ConverterExtension
    {
        public static WebAPI_NRE_Portal.Models.ProductionDataDto ToModel(this DataLayer_NRE_Portal.Models.ProductionData productionData)
        {
            return new WebAPI_NRE_Portal.Models.ProductionDataDto
            {
                Id = productionData.Id,
                Year = productionData.Year,
                ProductionKw = productionData.ProductionKWh,
                EnergyType = productionData.EnergyType,
                Region = productionData.Canton
            };
        }

        public static WebAPI_NRE_Portal.Models.PrivateInstallationDto MvcToApi(this MVC_NRE_Portal.Models.PrivateInstallationViewModel privateData)
        {
            return new WebAPI_NRE_Portal.Models.PrivateInstallationDto
            {
                EnergyType = privateData.EnergyType,
                IntegrationType = privateData.IntegrationType,
                PvCellType = privateData.PvCellType,
                Azimuth = privateData.Azimuth,
                RoofSlope = privateData.RoofSlope,
                Latitude = privateData.Latitude,
                Longitude = privateData.Longitude,
                LengthM = privateData.LengthM,
                WidthM = privateData.WidthM,
                AreaM2 = privateData.AreaM2,
                LocationText = privateData.Address,
            };
        }

        public static DataLayer_NRE_Portal.Models.PrivateInstallation ApiToDal(this WebAPI_NRE_Portal.Models.PrivateInstallationDto dto)
        {
            return new DataLayer_NRE_Portal.Models.PrivateInstallation
            {
                EnergyType = dto.EnergyType ?? string.Empty, // Avoid CS8601 by providing a default value
                IntegrationType = dto.IntegrationType,
                PvCellType = dto.PvCellType,
                Azimuth = dto.Azimuth,
                RoofSlope = dto.RoofSlope,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                LengthM = dto.LengthM,
                WidthM = dto.WidthM,
                AreaM2 = dto.AreaM2,
                LocationText = dto.LocationText,
                EstimatedKWh = dto.EstimatedKWh,
                InstalledCapacityKW = dto.EstimatedKWh ?? 0 // Avoid CS0266 and CS8629 by using null-coalescing and explicit conversion
            };
        }
    }
}
