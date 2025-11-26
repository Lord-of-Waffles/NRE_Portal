using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC_NRE_Portal.Models;
using WebAPI_NRE_Portal.Controllers;
using WebAPI_NRE_Portal.Models;
using WebAPI_NRE_Portal.Services;
using DataLayer_NRE_Portal.Data;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI_NRE_Portal.Tests.Controllers
{
    public class PrivateInstallationsControllerTests
    {
        [Fact]
        public async Task Create_CallsServiceMethod()
        {
            // Arrange
            var mockContext = new Mock<NrePortalContext>();
            var mockService = new Mock<IPrivateInstallationService>();
            var controller = new PrivateInstallationsController(mockContext.Object, mockService.Object);
            
            var viewModel = new PrivateInstallationViewModel();
            var createdDto = new PrivateInstallationDto { Id = 1 };
            
            mockService.Setup(s => s.CreateInstallationAsync(It.IsAny<PrivateInstallationDto>()))
                .ReturnsAsync(createdDto);

            // Act
            await controller.Create(viewModel);

            // Assert - Verify the service was called
            mockService.Verify(s => s.CreateInstallationAsync(It.IsAny<PrivateInstallationDto>()), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var mockContext = new Mock<NrePortalContext>();
            var mockService = new Mock<IPrivateInstallationService>();
            var controller = new PrivateInstallationsController(mockContext.Object, mockService.Object);
            
            var viewModel = new PrivateInstallationViewModel();
            var createdDto = new PrivateInstallationDto { Id = 1 };
            
            mockService.Setup(s => s.CreateInstallationAsync(It.IsAny<PrivateInstallationDto>()))
                .ReturnsAsync(createdDto);

            // Act
            var result = await controller.Create(viewModel);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}