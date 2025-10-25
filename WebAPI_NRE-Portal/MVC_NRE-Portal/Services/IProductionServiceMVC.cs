using MVC_NRE_Portal.Models;

namespace MVC_NRE_Portal.Services
{
    public interface IProductionServiceMVC
    {
        public Task<List<ProductionDataDto>> GetFakeYearData();
    }
}
