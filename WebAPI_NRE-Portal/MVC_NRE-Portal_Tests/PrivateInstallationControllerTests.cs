using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC_NRE_Portal.Controllers;
using MVC_NRE_Portal.Models;
using MVC_NRE_Portal.Services;
using System.Threading.Tasks;
using Xunit;

namespace MVC_NRE_Portal.Tests.Controllers
{
    public class PrivateInstallationControllerTests
    {
        private readonly Mock<IPrivateInstallationServiceMVC> _mockService;
        private readonly PrivateInstallationController _controller;

        public PrivateInstallationControllerTests()
        {
            _mockService = new Mock<IPrivateInstallationServiceMVC>();
            _controller = new PrivateInstallationController(_mockService.Object);
        }
        
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_SetsCorrectDefaultStep()
        {
            // Act
            var result = _controller.Index() as ViewResult;
            var model = result?.Model as PrivateInstallationViewModel;

            // Assert
            Assert.NotNull(model);
            Assert.Equal(1, model.CurrentStep);
        }


        [Fact]
        public void Navigate_Next_IncrementsStep()
        {
            // Arrange
            var model = new PrivateInstallationViewModel { CurrentStep = 1 };

            // Act
            var result = _controller.Navigate(model, "next") as ViewResult;
            var vm = result?.Model as PrivateInstallationViewModel;

            // Assert
            Assert.NotNull(vm);
            Assert.Equal(2, vm.CurrentStep);
        }

        [Fact]
        public void Navigate_Prev_DecrementsStep()
        {
            // Arrange
            var model = new PrivateInstallationViewModel { CurrentStep = 2 };

            // Act
            var result = _controller.Navigate(model, "prev") as ViewResult;
            var vm = result?.Model as PrivateInstallationViewModel;

            // Assert
            Assert.NotNull(vm);
            Assert.Equal(1, vm.CurrentStep);
        }

        [Fact]
        public void Navigate_DoesNotGoBelowOne()
        {
            var model = new PrivateInstallationViewModel { CurrentStep = 1 };

            var result = _controller.Navigate(model, "prev") as ViewResult;
            var vm = result?.Model as PrivateInstallationViewModel;

            Assert.Equal(1, vm.CurrentStep);
        }

        [Fact]
        public void Navigate_DoesNotExceedFour()
        {
            var model = new PrivateInstallationViewModel { CurrentStep = 4 };

            var result = _controller.Navigate(model, "next") as ViewResult;
            var vm = result?.Model as PrivateInstallationViewModel;

            Assert.Equal(4, vm.CurrentStep);
        }


        [Fact]
        public void Go_ChangesStepCorrectly()
        {
            // Arrange
            var carry = new PrivateInstallationViewModel { CurrentStep = 1 };

            // Act
            var result = _controller.Go(3, carry) as ViewResult;
            var vm = result?.Model as PrivateInstallationViewModel;

            // Assert
            Assert.NotNull(vm);
            Assert.Equal(3, vm.CurrentStep);
        }
        
    }
}
