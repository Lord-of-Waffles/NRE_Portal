using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI_NRE_Portal.Controllers;
using WebAPI_NRE_Portal.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI_NRE_Portal.Tests.Controllers
{
    public class ProductionSummariesControllerTests
    {
        [Fact]
        public async Task Get_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<NrePortalContext>();
            var mockService = new Mock<IProductionService>();
            var controller = new ProductionSummariesController(mockContext.Object, mockService.Object);
            
            var testData = new List<ProductionData>
            {
                new ProductionData(),
                new ProductionData()
            };
            
            mockService.Setup(s => s.GetProductionData("VS")).ReturnsAsync(testData);

            // Act
            var result = await controller.Get("VS");

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenNoData()
        {
            // Arrange
            var mockContext = new Mock<NrePortalContext>();
            var mockService = new Mock<IProductionService>();
            var controller = new ProductionSummariesController(mockContext.Object, mockService.Object);
            
            mockService.Setup(s => s.GetProductionData("VS"))
                .ReturnsAsync((IEnumerable<ProductionData>)null);

            // Act
            var result = await controller.Get("VS");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_CallsServiceWithCorrectParameter()
        {
            // Arrange
            var mockContext = new Mock<NrePortalContext>();
            var mockService = new Mock<IProductionService>();
            var controller = new ProductionSummariesController(mockContext.Object, mockService.Object);
            
            var testData = new List<ProductionData> { new ProductionData() };
            mockService.Setup(s => s.GetProductionData(It.IsAny<string>())).ReturnsAsync(testData);

            // Act
            await controller.Get("GE");

            // Assert - Verify service was called with "GE"
            mockService.Verify(s => s.GetProductionData("GE"), Times.Once);
        }
    }
}