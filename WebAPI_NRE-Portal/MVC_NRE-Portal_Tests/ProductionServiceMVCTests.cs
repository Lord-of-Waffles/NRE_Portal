using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MVC_NRE_Portal.Models;
using Moq;
using Moq.Protected;
using System.Net;
using Microsoft.Extensions.Configuration;
using MVC_NRE_Portal.Services;

namespace MVC_NRE_Portal_Tests
{
    public class ProductionServiceMVCTests
    {
        [Fact]
        public async Task GetProductionSummary_Should_Return_List_ProductionDataDto()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();

            HttpRequestMessage? capturedRequest = null;

            var fakeProductionSummary = new List<ProductionDataDto>
            {
                new ProductionDataDto
                {
                    Id = 1,
                    Year = 2005,
                    ProductionKw = 10000.0,
                    EnergyType = "Wind energy",
                    Region = "VS"
                },
                
                new ProductionDataDto
                {
                    Id = 2,
                    Year = 2008,
                    ProductionKw = 25000.0,
                    EnergyType = "Photovoltaic",
                    Region = "VS"
                }
            };

            var json = JsonSerializer.Serialize(fakeProductionSummary);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((req, _) =>
                {
                    capturedRequest = req;
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(handlerMock.Object);

            // Act
            var configDict = new Dictionary<string, string?>
            {
                { "WebAPI:BaseUrl", "https://fake-api.com" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();

            var service = new ProductionServiceMVC(httpClient, configuration);

            var result = await service.GetProductionSummary();

            // Assert

            Assert.NotNull(result);
            Assert.NotNull(capturedRequest);
            Assert.Equal(HttpMethod.Get, capturedRequest!.Method);
            Assert.Equal("https://fake-api.com/ProductionSummaries", capturedRequest!.RequestUri!.ToString());
            Assert.IsType<List<ProductionDataDto>>(result);
            Assert.Equal(fakeProductionSummary.Count, result.Count);

            for (int i = 0; i < fakeProductionSummary.Count; i++)
            {
                Assert.Equal(fakeProductionSummary[i].Id, result[i].Id);
                Assert.Equal(fakeProductionSummary[i].Year, result[i].Year);
                Assert.Equal(fakeProductionSummary[i].ProductionKw, result[i].ProductionKw);
                Assert.Equal(fakeProductionSummary[i].EnergyType, result[i].EnergyType);
                Assert.Equal(fakeProductionSummary[i].Region, result[i].Region);
            }
        }
    }
}
