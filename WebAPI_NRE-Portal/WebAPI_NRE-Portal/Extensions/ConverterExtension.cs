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

        public static DataLayer_NRE_Portal.Models.PrivateInstallation ApiToDal(this WebAPI_NRE_Portal.Models.PrivateInstallationDto dto)
        {
            return new DataLayer_NRE_Portal.Models.PrivateInstallation
            {
                // FROM INSTALLATIONBASE - ADD THESE
                Name = dto.Name ?? string.Empty,
                EnergyType = dto.EnergyType ?? string.Empty,
                Region = dto.Region ?? "VS",
                InstalledCapacityKW = dto.InstalledCapacityKW,
                AnnualProductionKWh = dto.AnnualProductionKWh,
                CommissioningDate = dto.CommissioningDate,  // ← KEY FIELD!
        
                // PRIVATE INSTALLATION SPECIFIC
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
                EstimatedKWh = dto.EstimatedKWh
            };
        }
    }
}