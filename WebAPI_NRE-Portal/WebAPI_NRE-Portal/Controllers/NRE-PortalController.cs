using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_NRE_Portal.Services;

namespace WebAPI_NRE_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NRE_PortalController : ControllerBase
    {
        private readonly IProductionService _productionService;
        public NRE_PortalController(IProductionService productionService)
        {
            _productionService = productionService;
        }

        [HttpGet]
        public  ActionResult GetFakeYearProductionData()
        {
            var data = _productionService.GetFakeYearProduction();
            if (data == null)
                return NotFound();
            return Ok(data);
            
        }
    }
}
