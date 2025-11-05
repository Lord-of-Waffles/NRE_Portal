using DataLayer_NRE_Portal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_NRE_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionSummariesController : ControllerBase
    {
        private readonly NrePortalContext _ctx;
        public ProductionSummariesController(NrePortalContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string canton = "VS")
        {
            var rows = await _ctx.ProductionSummaries
                .AsNoTracking()
                .Where(x => x.Canton == canton)
                .OrderBy(x => x.Year)
                .ToListAsync();

            return Ok(rows);
        }

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
