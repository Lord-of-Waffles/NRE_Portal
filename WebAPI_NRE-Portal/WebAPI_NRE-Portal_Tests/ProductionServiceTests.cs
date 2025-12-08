using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using WebAPI_NRE_Portal.Services;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;

namespace WebAPI_NRE_Portal.Tests
{
    public class ProductionServiceTests
    {
        private Mock<NrePortalContext> CreateMockContext(
            List<ProductionData> productionSummaries,
            List<PrivateInstallation> privateInstallations)
        {
            var mockContext = new Mock<NrePortalContext>();

            var mockProductionSet = CreateMockDbSet(productionSummaries);
            mockContext.Setup(c => c.ProductionSummaries).Returns(mockProductionSet.Object);

            var mockPrivateSet = CreateMockDbSet(privateInstallations);
            mockContext.Setup(c => c.PrivateInstallations).Returns(mockPrivateSet.Object);

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
        public async Task GetProductionData_CombinesHistoricalAndPrivateData_ForSameYearAndType()
        {
            // Arrange
            var productionSummaries = new List<ProductionData>
            {
                new ProductionData { Year = 2015, EnergyType = "Solar", ProductionKWh = 1000, Canton = "VS" }
            };
            var privateInstallations = new List<PrivateInstallation>
            {
                new PrivateInstallation 
                { 
                    Region = "VS", 
                    EnergyType = "Solar", 
                    CommissioningDate = new DateTime(2015, 6, 1),
                    AnnualProductionKWh = 500
                }
            };

            var mockContext = CreateMockContext(productionSummaries, privateInstallations);
            var service = new ProductionService(mockContext.Object);

            // Act
            var result = await service.GetProductionData("VS");

            // Assert
            var solarData = result.First(x => x.Year == 2015 && x.EnergyType == "Solar");
            Assert.Equal(1500, solarData.ProductionKWh);
        }

        [Fact]
        public async Task GetProductionData_AggregatesMultiplePrivateInstallationsBySameYearAndType()
        {
            // Arrange
            var productionSummaries = new List<ProductionData>();
            var privateInstallations = new List<PrivateInstallation>
            {
                new PrivateInstallation 
                { 
                    Region = "VS", 
                    EnergyType = "Solar", 
                    CommissioningDate = new DateTime(2020, 3, 1),
                    AnnualProductionKWh = 300
                },
                new PrivateInstallation 
                { 
                    Region = "VS", 
                    EnergyType = "Solar", 
                    CommissioningDate = new DateTime(2020, 8, 15),
                    AnnualProductionKWh = 400
                },
                new PrivateInstallation 
                { 
                    Region = "VS", 
                    EnergyType = "Wind", 
                    CommissioningDate = new DateTime(2020, 5, 1),
                    AnnualProductionKWh = 200
                }
            };

            var mockContext = CreateMockContext(productionSummaries, privateInstallations);
            var service = new ProductionService(mockContext.Object);

            // Act
            var result = await service.GetProductionData("VS");

            // Assert
            Assert.Equal(2, result.Count());
            var solarData = result.First(x => x.Year == 2020 && x.EnergyType == "Solar");
            Assert.Equal(700, solarData.ProductionKWh);
        }

        [Fact]
        public async Task GetProductionData_IgnoresInstallationsWithoutCommissioningDate()
        {
            // Arrange
            var productionSummaries = new List<ProductionData>();
            var privateInstallations = new List<PrivateInstallation>
            {
                new PrivateInstallation 
                { 
                    Region = "VS", 
                    EnergyType = "Solar", 
                    CommissioningDate = new DateTime(2020, 1, 1),
                    AnnualProductionKWh = 500
                },
                new PrivateInstallation 
                { 
                    Region = "VS", 
                    EnergyType = "Solar", 
                    CommissioningDate = null,
                    AnnualProductionKWh = 300
                }
            };

            var mockContext = CreateMockContext(productionSummaries, privateInstallations);
            var service = new ProductionService(mockContext.Object);

            // Act
            var result = await service.GetProductionData("VS");

            // Assert
            Assert.Single(result);
            Assert.Equal(500, result.First().ProductionKWh);
        }

        [Fact]
        public async Task GetProductionData_ReturnsEmptyList_WhenNoDataExists()
        {
            // Arrange
            var productionSummaries = new List<ProductionData>();
            var privateInstallations = new List<PrivateInstallation>();

            var mockContext = CreateMockContext(productionSummaries, privateInstallations);
            var service = new ProductionService(mockContext.Object);

            // Act
            var result = await service.GetProductionData("GE");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductionData_UsesEstimatedKWh_WhenAnnualProductionIsNull()
        {
            // Arrange
            var productionSummaries = new List<ProductionData>();
            var privateInstallations = new List<PrivateInstallation>
            {
                new PrivateInstallation 
                { 
                    Region = "VS", 
                    EnergyType = "Wind", 
                    CommissioningDate = new DateTime(2021, 1, 1),
                    AnnualProductionKWh = null,
                    EstimatedKWh = 800
                }
            };

            var mockContext = CreateMockContext(productionSummaries, privateInstallations);
            var service = new ProductionService(mockContext.Object);

            // Act
            var result = await service.GetProductionData("VS");

            // Assert
            var windData = result.First();
            Assert.Equal(800, windData.ProductionKWh);
        }

        [Fact]
        public void GetFakeYearProduction_Returns10Records()
        {
            // Arrange
            var mockContext = new Mock<NrePortalContext>();
            var service = new ProductionService(mockContext.Object);

            // Act
            var result = service.GetFakeYearProduction();

            // Assert
            Assert.Equal(10, result.Count);
        }
    }
}