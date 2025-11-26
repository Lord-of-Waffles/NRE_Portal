using DataLayer_NRE_Portal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_NRE_Portal.Extensions;
using WebAPI_NRE_Portal.Services;
using WebAPI_NRE_Portal.Models;

namespace WebAPI_NRE_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionSummariesController : ControllerBase
    {
        private readonly NrePortalContext _ctx;
        private readonly IProductionService _productionService;
        public ProductionSummariesController(NrePortalContext ctx, IProductionService productionService)
        {
            _ctx = ctx;
            _productionService = productionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string canton = "VS")
        {
            var rows = await _productionService.GetProductionData(canton);
            if(rows == null)
            {
                return NotFound();
            }
            var productionDataDtos = new List<Models.ProductionDataDto>();
            foreach(var row in rows)
            {
                Models.ProductionDataDto productionDataDto = new Models.ProductionDataDto();
                productionDataDto = row.ToModel();
                productionDataDtos.Add(productionDataDto);
            }

            return Ok(productionDataDtos);
        }

        /*[HttpGet]
        public async Task<IActionResult> Get([FromQuery] string canton = "VS")
        {
            var rows = await _ctx.ProductionSummaries
                .AsNoTracking()
                .Where(x => x.Canton == canton)
                .OrderBy(x => x.Year)
                .ToListAsync();

            return Ok(rows);
        }   */

        [HttpGet("totals-by-energy")]
        public async Task<IActionResult> TotalsByEnergy([FromQuery] string canton = "VS")
        {
            var rows = await _ctx.ProductionSummaries
                .AsNoTracking()
                .Where(x => x.Canton == canton)
                .GroupBy(x => x.EnergyType)
                .Select(g => new { EnergyType = g.Key, KWh = g.Sum(r => r.ProductionKWh) })
                .OrderByDescending(x => x.KWh)
                .ToListAsync();

            return Ok(rows);
        }
    }
}
