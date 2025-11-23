using DataLayer_NRE_Portal.Data;
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
    }
}
