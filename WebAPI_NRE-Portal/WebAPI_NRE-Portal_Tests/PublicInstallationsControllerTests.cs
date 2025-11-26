using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI_NRE_Portal.Controllers;
using DataLayer_NRE_Portal.Data;
using Xunit;

namespace WebAPI_NRE_Portal.Tests.Controllers
{
    public class PublicInstallationsControllerTests
    {
        [Fact]
        public void Constructor_WithValidContext_CreatesInstance()
        {
            // Arrange
            var mockContext = new Mock<NrePortalContext>();

            // Act
            var controller = new PublicInstallationsController(mockContext.Object);

            // Assert
            Assert.NotNull(controller);
        }
        
    }
}