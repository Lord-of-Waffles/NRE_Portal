using MVC_NRE_Portal.Models;

namespace MVC_NRE_Portal.Services
{
    public class PrivateInstallationServiceMVC : IPrivateInstallationServiceMVC
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public PrivateInstallationServiceMVC(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _baseUrl = configuration["WebAPI:BaseUrl"];
        }

        public async Task PostPrivateInstallation(PrivateInstallationViewModel vm)
        {
            // Convert ViewModel to DTO
            var dto = new PrivateInstallationDto
            {
                EnergyType = vm.EnergyType,
                IntegrationType = vm.IntegrationType,
                PvCellType = vm.PvCellType,
                Azimuth = vm.Azimuth,
                RoofSlope = vm.RoofSlope,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                LengthM = vm.LengthM,
                WidthM = vm.WidthM,
                AreaM2 = vm.AreaM2,
                LocationText = vm.Address,
                InstalledCapacityKW = vm.InstalledCapacityKW
            };
            
            await _http.PostAsJsonAsync(_baseUrl + "/PrivateInstallations", dto);
        }
    }
}