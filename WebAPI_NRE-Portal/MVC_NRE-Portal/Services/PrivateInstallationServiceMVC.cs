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
            await _http.PostAsJsonAsync(_baseUrl + "/PrivateInstallations", vm);
        }
    }
}
