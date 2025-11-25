using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC_NRE_Portal.Controllers;
using MVC_NRE_Portal.Models;
using MVC_NRE_Portal.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MVC_NRE_Portal.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<IProductionServiceMVC> _mockService;
        private readonly HomeController _controller;

        // Constructor runs before each test
        public HomeControllerTests()
        {
            _mockService = new Mock<IProductionServiceMVC>();
            _controller = new HomeController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange
            _mockService.Setup(s => s.GetFakeYearData())
                       .ReturnsAsync(GetTestProductionData());
            
            // Act
            var result = await _controller.Index();
            
            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsViewWithChartViewModel()
        {
            // Arrange
            _mockService.Setup(s => s.GetFakeYearData())
                       .ReturnsAsync(GetTestProductionData());
            
            // Act
            var result = await _controller.Index() as ViewResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ChartViewModel>(result.Model);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy();
            
            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsErrorViewWithRequestId()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "test-trace-12345";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            
            // Act
            var result = _controller.Error() as ViewResult;
            var model = result?.Model as ErrorViewModel;
            
            // Assert
            Assert.NotNull(model);
            Assert.Equal("test-trace-12345", model.RequestId);
        }

       
        private List<ProductionDataDto> GetTestProductionData()
        {
            return new List<ProductionDataDto>
            {
                new ProductionDataDto { Year = 2010, EnergyType = "PV", ProductionKw = 1_000_000, Region = "VS" },
                new ProductionDataDto { Year = 2010, EnergyType = "Biogas", ProductionKw = 3_000_000, Region = "VS" }
            };
        }
    }
}