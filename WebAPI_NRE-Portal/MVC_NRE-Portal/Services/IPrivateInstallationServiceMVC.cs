using Microsoft.AspNetCore.Mvc;
using MVC_NRE_Portal.Models;

namespace MVC_NRE_Portal.Services
{
    public interface IPrivateInstallationServiceMVC
    {
        Task PostPrivateInstallation(PrivateInstallationViewModel viewModel);
    }
}
