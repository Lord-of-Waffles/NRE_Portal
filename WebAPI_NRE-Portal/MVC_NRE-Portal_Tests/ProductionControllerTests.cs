using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC_NRE_Portal.Controllers;
using MVC_NRE_Portal.Models;
using MVC_NRE_Portal.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MVC_NRE_Portal.Tests.Controllers
{
    public class ProductionControllerTests
    {
        private readonly Mock<IProductionServiceMVC> _mockService;
        private readonly ProductionController _controller;

        public ProductionControllerTests()
        {
            _mockService = new Mock<IProductionServiceMVC>();
            _controller = new ProductionController(_mockService.Object);
        }

        [Fact]
        public async Task Dashboard_ReturnsViewResult()
        {
            _mockService.Setup(s => s.GetProductionSummary())
                .ReturnsAsync(GetTestData());

            var result = await _controller.Dashboard();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Dashboard_ReturnsChartViewModel()
        {
            _mockService.Setup(s => s.GetProductionSummary())
                .ReturnsAsync(GetTestData());

            var result = await _controller.Dashboard() as ViewResult;
            Assert.NotNull(result);

            Assert.IsType<ChartViewModel>(result.Model);
        }

        [Fact]
        public async Task Dashboard_GroupsByEnergyTypeCorrectly()
        {
            _mockService.Setup(s => s.GetProductionSummary())
                .ReturnsAsync(GetTestData());

            var result = await _controller.Dashboard() as ViewResult;
            var vm = result?.Model as ChartViewModel;

            Assert.NotNull(vm);

            Assert.Contains("PV", vm.Labels);
            Assert.Contains("Biogas", vm.Labels);

            Assert.Contains(400, vm.Data);
            Assert.Contains(200, vm.Data);
        }

        [Fact]
        public async Task PV_ReturnsEnergyPageView()
        {
            _mockService.Setup(s => s.GetProductionSummary())
                .ReturnsAsync(GetTestData());

            var result = await _controller.PV() as ViewResult;

            Assert.Equal("EnergyPage", result?.ViewName);
            Assert.IsType<ChartViewModel>(result?.Model);
        }

        [Fact]
        public async Task MiniHydro_ReturnsEnergyPageView()
        {
            _mockService.Setup(s => s.GetProductionSummary())
                .ReturnsAsync(GetTestData());

            var result = await _controller.MiniHydro() as ViewResult;

            Assert.Equal("EnergyPage", result?.ViewName);
        }

        [Fact]
        public async Task Wind_ReturnsEnergyPageView()
        {
            _mockService.Setup(s => s.GetProductionSummary())
                .ReturnsAsync(GetTestData());

            var result = await _controller.Wind() as ViewResult;

            Assert.Equal("EnergyPage", result?.ViewName);
        }

        [Fact]
        public async Task Biogas_ReturnsEnergyPageView()
        {
            _mockService.Setup(s => s.GetProductionSummary())
                .ReturnsAsync(GetTestData());

            var result = await _controller.Biogas() as ViewResult;

            Assert.Equal("EnergyPage", result?.ViewName);
        }



        private List<ProductionDataDto> GetTestData()
        {
            return new List<ProductionDataDto>
            {
                new ProductionDataDto { Year = 2020, EnergyType = "PV", ProductionKw = 100 },
                new ProductionDataDto { Year = 2021, EnergyType = "PV", ProductionKw = 300 },
                new ProductionDataDto { Year = 2020, EnergyType = "Biogas", ProductionKw = 200 },
                new ProductionDataDto { Year = 2020, EnergyType = "Wind energy", ProductionKw = 50 },
                new ProductionDataDto { Year = 2020, EnergyType = "Hydroelectric power", ProductionKw = 75 }
            };
        }
    }
}
