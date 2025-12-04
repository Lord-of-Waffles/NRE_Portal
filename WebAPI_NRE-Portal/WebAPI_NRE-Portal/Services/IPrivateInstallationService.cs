using WebAPI_NRE_Portal.Models;

namespace WebAPI_NRE_Portal.Services
{
    public interface IPrivateInstallationService
    {
        Task<PrivateInstallationDto> CreateInstallationAsync(PrivateInstallationDto dto);
        Task<List<PrivateInstallationDto>> GetInstallationsAsync();
        Task<PrivateInstallationDto?> GetByIdAsync(int id);
    }
}
