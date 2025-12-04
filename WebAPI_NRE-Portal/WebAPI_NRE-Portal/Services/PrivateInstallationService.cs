using DataLayer_NRE_Portal.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI_NRE_Portal.Extensions;
using WebAPI_NRE_Portal.Models;

namespace WebAPI_NRE_Portal.Services
{
    public class PrivateInstallationService : IPrivateInstallationService
    {
        private readonly NrePortalContext _context;
        public PrivateInstallationService(NrePortalContext context)
        {
            _context = context;
        }

        public async Task<PrivateInstallationDto> CreateInstallationAsync(PrivateInstallationDto dto)
        {
            // Calculs métiers sur le DTO
            if (!dto.AreaM2.HasValue && dto.LengthM.HasValue && dto.WidthM.HasValue)
                dto.AreaM2 = dto.LengthM.Value * dto.WidthM.Value;

            if (string.Equals(dto.EnergyType, "PV", StringComparison.OrdinalIgnoreCase))
            {
                double perM2 = dto.PvCellType?.Equals("Monocrystalline", StringComparison.OrdinalIgnoreCase) == true ? 250 : 175;
                double az = dto.Azimuth ?? 0;
                double orientationFactor = Math.Abs(az) <= 15 ? 1.0 : (Math.Abs(az) >= 75 ? 0.8 : 0.9);
                dto.EstimatedKWh = (dto.AreaM2 ?? 0) * perM2 * orientationFactor;
            }

            var entity = dto.ApiToDal();

            _context.PrivateInstallations.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }
        
        public async Task<List<PrivateInstallationDto>> GetInstallationsAsync()
        {
            var installations = await _context.PrivateInstallations.ToListAsync();
        
            // Map to DTOs
            return installations.Select(i => new PrivateInstallationDto
            {
                Id = i.Id,
                Name = i.Name,
                EnergyType = i.EnergyType,
                Region = i.Region,
                InstalledCapacityKW = i.InstalledCapacityKW,
                AnnualProductionKWh = i.AnnualProductionKWh,
                CommissioningDate =  i.CommissioningDate,
                IntegrationType = i.IntegrationType,
                PvCellType = i.PvCellType,
                Azimuth = i.Azimuth,
                RoofSlope =  i.RoofSlope,
                Latitude = i.Latitude,
                Longitude = i.Longitude,
                LengthM = i.LengthM,
                WidthM = i.WidthM,
                AreaM2 = i.AreaM2,
                EstimatedKWh =  i.EstimatedKWh,
                LocationText =  i.LocationText
            }).ToList();
        }
        
        public async Task<PrivateInstallationDto?> GetByIdAsync(int id)
        {
            var installation = await _context.PrivateInstallations.FindAsync(id);
    
            if (installation == null)
                return null;
    
            return new PrivateInstallationDto
            {
                Id = installation.Id,
                Name = installation.Name,
                EnergyType = installation.EnergyType,
                Region = installation.Region,
                InstalledCapacityKW = installation.InstalledCapacityKW,
                AnnualProductionKWh = installation.AnnualProductionKWh,
                CommissioningDate =  installation.CommissioningDate,
                IntegrationType = installation.IntegrationType,
                PvCellType = installation.PvCellType,
                Azimuth = installation.Azimuth,
                RoofSlope =  installation.RoofSlope,
                Latitude = installation.Latitude,
                Longitude = installation.Longitude,
                LengthM = installation.LengthM,
                WidthM = installation.WidthM,
                AreaM2 = installation.AreaM2,
                EstimatedKWh =  installation.EstimatedKWh,
                LocationText =  installation.LocationText
            };
        }
        
        
    }
}
