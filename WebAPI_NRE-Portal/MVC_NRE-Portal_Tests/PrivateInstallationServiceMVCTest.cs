using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MVC_NRE_Portal.Models;
using MVC_NRE_Portal.Services;
using System.Text.Json;

namespace MVC_NRE_Portal_Tests
{
    public class PrivateInstallationServiceMVCTest
    {
        [Fact]
        public async Task PostPrivateInstallation_Should_Send_Correct_Request()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();

            HttpRequestMessage? captureRequest = null;

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((req, _) =>
                {
                    captureRequest = req;
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
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

            var service = new PrivateInstallationServiceMVC(httpClient, configuration);

            var vm = new PrivateInstallationViewModel
            {
                EnergyType = "Wind Energy",
                IntegrationType = "Integrated",
                PvCellType = "Monocrystalline",
                Azimuth = 35,
                RoofSlope = 12,
                Latitude = 48.5,
                Longitude = 2.3,
                LengthM = 10,
                WidthM = 5,
                AreaM2 = 50,
                Address = "123 rue du soleil",
                InstalledCapacityKW = 6.2
            };

            await service.PostPrivateInstallation(vm);

            // Assert

            Assert.NotNull(captureRequest);
            Assert.Equal(HttpMethod.Post, captureRequest!.Method);
            Assert.Equal("https://fake-api.com/PrivateInstallations", captureRequest.RequestUri!.ToString());

            var json = await captureRequest.Content!.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var sentDto = JsonSerializer.Deserialize<PrivateInstallationDto>(json, option);

            Assert.Equal(vm.EnergyType, sentDto!.EnergyType);
            Assert.Equal(vm.IntegrationType, sentDto.IntegrationType);
            Assert.Equal(vm.PvCellType, sentDto.PvCellType);
            Assert.Equal(vm.Azimuth, sentDto.Azimuth);
            Assert.Equal(vm.RoofSlope, sentDto.RoofSlope);
            Assert.Equal(vm.Latitude, sentDto.Latitude);
            Assert.Equal(vm.Longitude, sentDto.Longitude);
            Assert.Equal(vm.LengthM, sentDto.LengthM);
            Assert.Equal(vm.WidthM, sentDto.WidthM);
            Assert.Equal(vm.AreaM2, sentDto.AreaM2);
            Assert.Equal(vm.Address, sentDto.LocationText);
            Assert.Equal(vm.InstalledCapacityKW, sentDto.InstalledCapacityKW);
        }
    }
}
