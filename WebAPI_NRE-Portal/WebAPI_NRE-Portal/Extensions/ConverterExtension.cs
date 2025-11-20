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
    }
}
