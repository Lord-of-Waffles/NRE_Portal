using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var installations = await _privateService.GetInstallationsAsync();
    
            if (!string.IsNullOrWhiteSpace(region)) 
                installations = installations.Where(x => x.Region == region).ToList();
    
            if (!string.IsNullOrWhiteSpace(energy)) 
                installations = installations.Where(x => x.EnergyType == energy).ToList();
    
            return Ok(installations);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _ctx.PrivateInstallations.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrivateInstallationDto dto)  // ← Changed to DTO
        {
            var createdDto = await _privateService.CreateInstallationAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);  
        }
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            var allPrivate = await _ctx.PrivateInstallations.ToListAsync();
            _ctx.PrivateInstallations.RemoveRange(allPrivate);
            await _ctx.SaveChangesAsync();
            return Ok(new { message = $"Deleted {allPrivate.Count} private installations" });
        }
    }
}