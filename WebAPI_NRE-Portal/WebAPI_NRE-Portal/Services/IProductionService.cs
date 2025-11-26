using DataLayer_NRE_Portal.Models;
namespace WebAPI_NRE_Portal.Services
{
    public interface IProductionService
    {
        List<ProductionDataDto> GetFakeYearProduction();
        Task<IEnumerable<ProductionData>> GetProductionData(string canton);

    }
}
