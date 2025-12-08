using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using WebAPI_NRE_Portal.Models;
using WebAPI_NRE_Portal.Services;
using Xunit;

namespace WebAPI_NRE_Portal.Tests
{
    public class PrivateInstallationServiceTests
    {
        private Mock<NrePortalContext> CreateMockContext(List<PrivateInstallation> privateInstallations)
        {
            var mockContext = new Mock<NrePortalContext>();
            var mockSet = CreateMockDbSet(privateInstallations);
            
            mockContext.Setup(c => c.PrivateInstallations).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            return mockContext;
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            return mockSet;
        }

        [Fact]
        public async Task CreateInstallationAsync_CalculatesAreaFromLengthAndWidth()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            var dto = new PrivateInstallationDto
            {
                Name = "Test Solar",
                EnergyType = "Solar",
                LengthM = 10,
                WidthM = 5,
                AreaM2 = null
            };

            // Act
            var result = await service.CreateInstallationAsync(dto);

            // Assert
            Assert.Equal(50, result.AreaM2);
        }

        [Fact]
        public async Task CreateInstallationAsync_DoesNotRecalculateAreaIfAlreadySet()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            var dto = new PrivateInstallationDto
            {
                Name = "Test Solar",
                EnergyType = "Solar",
                LengthM = 10,
                WidthM = 5,
                AreaM2 = 100 // Already set
            };

            // Act
            var result = await service.CreateInstallationAsync(dto);

            // Assert
            Assert.Equal(100, result.AreaM2); // Should keep original value
        }

        [Fact]
        public async Task CreateInstallationAsync_CalculatesEstimatedKWh_ForMonocrystallinePV_SouthFacing()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            var dto = new PrivateInstallationDto
            {
                Name = "Test Solar",
                EnergyType = "PV",
                PvCellType = "Monocrystalline",
                AreaM2 = 20,
                Azimuth = 0 // Perfect south facing
            };

            // Act
            var result = await service.CreateInstallationAsync(dto);

            // Assert
            // 20 m² * 250 kWh/m² * 1.0 (orientation factor) = 5000
            Assert.Equal(5000, result.EstimatedKWh);
        }

        [Fact]
        public async Task CreateInstallationAsync_CalculatesEstimatedKWh_ForPolycrystallinePV()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            var dto = new PrivateInstallationDto
            {
                Name = "Test Solar",
                EnergyType = "PV",
                PvCellType = "Polycrystalline",
                AreaM2 = 20,
                Azimuth = 0
            };

            // Act
            var result = await service.CreateInstallationAsync(dto);

            // Assert
            // 20 m² * 175 kWh/m² * 1.0 = 3500
            Assert.Equal(3500, result.EstimatedKWh);
        }

        [Fact]
        public async Task CreateInstallationAsync_AppliesOrientationFactor_ForModerateDeviation()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            var dto = new PrivateInstallationDto
            {
                Name = "Test Solar",
                EnergyType = "PV",
                PvCellType = "Monocrystalline",
                AreaM2 = 20,
                Azimuth = 45 // Moderate deviation (15 < abs(45) < 75)
            };

            // Act
            var result = await service.CreateInstallationAsync(dto);

            // Assert
            // 20 m² * 250 kWh/m² * 0.9 = 4500
            Assert.Equal(4500, result.EstimatedKWh);
        }

        [Fact]
        public async Task CreateInstallationAsync_AppliesOrientationFactor_ForLargeDeviation()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            var dto = new PrivateInstallationDto
            {
                Name = "Test Solar",
                EnergyType = "PV",
                PvCellType = "Monocrystalline",
                AreaM2 = 20,
                Azimuth = 90 // Large deviation (abs(90) >= 75)
            };

            // Act
            var result = await service.CreateInstallationAsync(dto);

            // Assert
            // 20 m² * 250 kWh/m² * 0.8 = 4000
            Assert.Equal(4000, result.EstimatedKWh);
        }

        [Fact]
        public async Task CreateInstallationAsync_DoesNotCalculateEstimatedKWh_ForNonPVType()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            var dto = new PrivateInstallationDto
            {
                Name = "Test Wind",
                EnergyType = "Wind",
                AreaM2 = 20
            };

            // Act
            var result = await service.CreateInstallationAsync(dto);

            // Assert
            Assert.Null(result.EstimatedKWh);
        }

        [Fact]
        public async Task GetInstallationsAsync_ReturnsAllInstallations()
        {
            // Arrange
            var installations = new List<PrivateInstallation>
            {
                new PrivateInstallation { Id = 1, Name = "Installation 1", EnergyType = "Solar" },
                new PrivateInstallation { Id = 2, Name = "Installation 2", EnergyType = "Wind" },
                new PrivateInstallation { Id = 3, Name = "Installation 3", EnergyType = "Hydro" }
            };

            var mockContext = CreateMockContext(installations);
            var service = new PrivateInstallationService(mockContext.Object);

            // Act
            var result = await service.GetInstallationsAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Installation 1", result[0].Name);
            Assert.Equal("Installation 2", result[1].Name);
            Assert.Equal("Installation 3", result[2].Name);
        }

        [Fact]
        public async Task GetInstallationsAsync_ReturnsEmptyList_WhenNoInstallations()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            // Act
            var result = await service.GetInstallationsAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsInstallation_WhenExists()
        {
            // Arrange
            var installation = new PrivateInstallation 
            { 
                Id = 1, 
                Name = "Test Installation", 
                EnergyType = "Solar",
                Region = "VS"
            };

            var mockContext = CreateMockContext(new List<PrivateInstallation> { installation });
            mockContext.Setup(c => c.PrivateInstallations.FindAsync(1))
                .ReturnsAsync(installation);

            var service = new PrivateInstallationService(mockContext.Object);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Installation", result.Name);
            Assert.Equal("Solar", result.EnergyType);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            mockContext.Setup(c => c.PrivateInstallations.FindAsync(999))
                .ReturnsAsync((PrivateInstallation?)null);

            var service = new PrivateInstallationService(mockContext.Object);

            // Act
            var result = await service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAllAsync_RemovesAllInstallations()
        {
            // Arrange
            var installations = new List<PrivateInstallation>
            {
                new PrivateInstallation { Id = 1, Name = "Installation 1" },
                new PrivateInstallation { Id = 2, Name = "Installation 2" }
            };

            var mockContext = CreateMockContext(installations);
            var service = new PrivateInstallationService(mockContext.Object);

            // Act
            await service.DeleteAllAsync();

            // Assert
            mockContext.Verify(c => c.PrivateInstallations.RemoveRange(It.IsAny<IEnumerable<PrivateInstallation>>()), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateInstallationAsync_CombinesAreaCalculationAndEstimation()
        {
            // Arrange
            var mockContext = CreateMockContext(new List<PrivateInstallation>());
            var service = new PrivateInstallationService(mockContext.Object);

            var dto = new PrivateInstallationDto
            {
                Name = "Test Solar",
                EnergyType = "PV",
                PvCellType = "Monocrystalline",
                LengthM = 10,
                WidthM = 4,
                Azimuth = 10
            };

            // Act
            var result = await service.CreateInstallationAsync(dto);

            // Assert
            Assert.Equal(40, result.AreaM2); // 10 * 4
            Assert.Equal(10000, result.EstimatedKWh); // 40 * 250 * 1.0
        }
    }
}