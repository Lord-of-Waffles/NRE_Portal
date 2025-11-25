using Microsoft.Extensions.Logging;
using Moq;
using WebAPI_NRE_Portal.Controllers;
using Xunit;

namespace WebAPI_NRE_Portal.Tests.Controllers
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void Get_ReturnsForecasts()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(mockLogger.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Get_ReturnsFiveForecasts()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(mockLogger.Object);

            // Act
            var result = controller.Get();

            // Assert
            var forecasts = result.ToList();
            Assert.Equal(5, forecasts.Count);
        }
    }
}
