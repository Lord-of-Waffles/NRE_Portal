using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_NRE_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicInstallationsController : ControllerBase
    {
        private readonly NrePortalContext _ctx;
        public PublicInstallationsController(NrePortalContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? region = "VS", [FromQuery] string? energy = null)
        {
            var q = _ctx.PublicInstallations.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(region)) q = q.Where(x => x.Region == region);
            if (!string.IsNullOrWhiteSpace(energy)) q = q.Where(x => x.EnergyType == energy);
            return Ok(await q.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _ctx.PublicInstallations.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PublicInstallation plant)
        {
            _ctx.PublicInstallations.Add(plant);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = plant.Id }, plant);
        }
    }
}
