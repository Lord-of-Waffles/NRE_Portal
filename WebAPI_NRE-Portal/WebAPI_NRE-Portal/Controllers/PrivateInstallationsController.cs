using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_NRE_Portal.Models;
using WebAPI_NRE_Portal.Extensions;
using WebAPI_NRE_Portal.Models;
using WebAPI_NRE_Portal.Services;

namespace WebAPI_NRE_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrivateInstallationsController : ControllerBase
    {
        private readonly NrePortalContext _ctx;
        private readonly IPrivateInstallationService _privateService;
        public PrivateInstallationsController(NrePortalContext ctx, IPrivateInstallationService privateService)
        {
            _ctx = ctx;
            _privateService = privateService;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? region = "VS", [FromQuery] string? energy = null)
        {
            var q = _ctx.PrivateInstallations.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(region)) q = q.Where(x => x.Region == region);
            if (!string.IsNullOrWhiteSpace(energy)) q = q.Where(x => x.EnergyType == energy);
            return Ok(await q.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _ctx.PrivateInstallations.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        // Create with PV estimate if applicable
        [HttpPost]
        public async Task<IActionResult> Create(PrivateInstallationViewModel model)
        {
            var privateDto = model.MvcToApi();
            var createdDto = await _privateService.CreateInstallationAsync(privateDto);
            return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);  
        }   

     /*   // Create with PV estimate if applicable
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrivateInstallation model)
        {
            // compute area if needed
            if (!model.AreaM2.HasValue && model.LengthM.HasValue && model.WidthM.HasValue)
                model.AreaM2 = model.LengthM.Value * model.WidthM.Value;

            // PV rough estimate rule (from your PDF)
            if (string.Equals(model.EnergyType, "PV", StringComparison.OrdinalIgnoreCase))
            {
                double perM2 = model.PvCellType?.Equals("Monocrystalline", StringComparison.OrdinalIgnoreCase) == true ? 250 : 175;
                double az = model.Azimuth ?? 0;
                double orientationFactor = Math.Abs(az) <= 15 ? 1.0 : (Math.Abs(az) >= 75 ? 0.8 : 0.9); // simple step: south ~1.0, east/west ~0.8
                model.EstimatedKWh = (model.AreaM2 ?? 0) * perM2 * orientationFactor;
            }

            _ctx.PrivateInstallations.Add(model);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }   */
    }
}
