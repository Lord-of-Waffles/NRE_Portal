using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI_NRE_Portal.Controllers;
using WebAPI_NRE_Portal.Services;
using Xunit;

namespace WebAPI_NRE_Portal.Tests.Controllers
{
    public class NRE_PortalControllerTests
    {


        [Fact]
        public void GetFakeYearProductionData_ReturnsNotFound_WhenNoData()
        {
            // Arrange
            var mockService = new Mock<IProductionService>();
            var controller = new NRE_PortalController(mockService.Object);
            
            mockService.Setup(s => s.GetFakeYearProduction()).Returns(() => null);

            // Act
            var result = controller.GetFakeYearProductionData();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
