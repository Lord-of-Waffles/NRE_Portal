using System.Net.Http.Headers;
using DataLayer_NRE_Portal.Data;
using DataLayer_NRE_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI_NRE_Portal.Controllers;
using WebAPI_NRE_Portal.Models;
using Moq;
using WebAPI_NRE_Portal.Services;

namespace WebAPI_NRE_Portal.Tests.Controllers;

public class PrivateInstallationsControllerTests
{

    [Fact]
    public async Task get_api_privateinstallations_returns_ok()
    {
        var mockContext = new Mock<NrePortalContext>();
        var mockService = new Mock<IPrivateInstallationService>();
        var controller = new PrivateInstallationsController(mockContext.Object, mockService.Object);

        var fakeInstallations = new List<PrivateInstallationDto>
        {
            new PrivateInstallationDto
            {
                Id = 1,
                Name = "Number1",
                EnergyType = "Photovoltaic",
                Region = "VS",
                InstalledCapacityKW = 120,
                AnnualProductionKWh = 120,
                CommissioningDate = DateTime.Now,
                IntegrationType = "Added",
                PvCellType = "Mono",
                Azimuth = 20,
                RoofSlope = 40,
                Latitude = 2,
                Longitude = 2,
                LengthM = 20,
                WidthM = 20,
                AreaM2 = 400,
                EstimatedKWh = 2.0,
                LocationText = "on the street"
            },
            new PrivateInstallationDto
            {
                Id = 2,
                Name = "Number2",
                EnergyType = "Photovoltaic",
                Region = "VS",
                InstalledCapacityKW = 120,
                AnnualProductionKWh = 120,
                CommissioningDate = DateTime.Now,
                IntegrationType = "Added",
                PvCellType = "Mono",
                Azimuth = 20,
                RoofSlope = 40,
                Longitude = 2,
                LengthM = 20,
                WidthM = 20,
                AreaM2 = 400,
                EstimatedKWh = 2.0,
                LocationText = "on the street"
            }
        };
        mockService.Setup(s=>s.GetInstallationsAsync()).ReturnsAsync(fakeInstallations);
        
        var result = await controller.GetAll();
        
        var okResult = Assert.IsType<OkObjectResult>(result); 
        var returnedInstallations = Assert.IsType<List<PrivateInstallationDto>>(okResult.Value);
        Assert.Equal(2, returnedInstallations.Count);
        Assert.Equal(1, returnedInstallations[0].Id);
        Assert.Equal(2, returnedInstallations[1].Id);
    }

    [Fact]
    public async Task post_api_privateinstallations_returns_created_installations()
    {
        var mockContext = new Mock<NrePortalContext>();
        var mockService = new Mock<IPrivateInstallationService>();
        var controller = new PrivateInstallationsController(mockContext.Object, mockService.Object);

        var inputDto = new PrivateInstallationDto
        {
            Name = "Number3",
            EnergyType = "Photovoltaic",
            Region = "VS",
            InstalledCapacityKW = 120,
            AnnualProductionKWh = 120,
            CommissioningDate = DateTime.Now,
            IntegrationType = "Added",
            PvCellType = "Mono",
            Azimuth = 20,
            RoofSlope = 40,
            Longitude = 2,
            LengthM = 20,
            WidthM = 20,
            AreaM2 = 400,
            EstimatedKWh = 2.0,
            LocationText = "on the street"
        };
        var createdDto = new PrivateInstallationDto()
        {
            Id = 1,
            Name = "Number1",
            EnergyType = "Photovoltaic",
            Region = "VS",
            InstalledCapacityKW = 120,
            AnnualProductionKWh = 120,
            CommissioningDate = DateTime.Now,
            IntegrationType = "Added",
            PvCellType = "Mono",
            Azimuth = 20,
            RoofSlope = 40,
            Longitude = 2,
            LengthM = 20,
            WidthM = 20,
            AreaM2 = 400,
            EstimatedKWh = 2.0,
            LocationText = "on the street"
        };
        
        mockService.Setup(s=>s.CreateInstallationAsync(It.IsAny<PrivateInstallationDto>())).ReturnsAsync(createdDto);
        
        var result = await controller.Create(inputDto);
        
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);



        var returnedDto = Assert.IsType<PrivateInstallationDto>(createdResult.Value);
        Assert.Equal(createdDto.Id, returnedDto.Id);
        Assert.Equal(createdDto.Name, returnedDto.Name);
        Assert.Equal(createdDto.EnergyType, returnedDto.EnergyType);    }

    [Fact]
    public async Task get_api_privateinstallations_id_test()
    {
        var mockContext = new Mock<NrePortalContext>();
        var mockService = new Mock<IPrivateInstallationService>();
        var controller = new PrivateInstallationsController(mockContext.Object, mockService.Object);

        var fakeInstallation = new PrivateInstallationDto
        {
            Id = 1,
            Name = "Number1",
            EnergyType = "Photovoltaic",
            Region = "VS",
            InstalledCapacityKW = 120,
            AnnualProductionKWh = 120,
            CommissioningDate = DateTime.Now,
            IntegrationType = "Added",
            PvCellType = "Mono",
            Azimuth = 20,
            RoofSlope = 40,
            Latitude = 2,
            Longitude = 2,
            LengthM = 20,
            WidthM = 20,
            AreaM2 = 400,
            EstimatedKWh = 2.0,
            LocationText = "on the street"
        };
        mockService.Setup(s=>s.GetByIdAsync(1)).ReturnsAsync(fakeInstallation);
        
        var result = await controller.Get(1);
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<PrivateInstallationDto>(okResult.Value);
        Assert.Equal(1, returnedDto.Id);
        Assert.Equal("Number1", returnedDto.Name);
    }

    [Fact]
    public async Task delete_api_privateinstallations_all_test()
    {
        var mockContext = new Mock<NrePortalContext>();
        var mockService = new Mock<IPrivateInstallationService>();
        var controller = new PrivateInstallationsController(mockContext.Object, mockService.Object);
        
        mockService.Setup(s=>s.DeleteAllAsync()).Returns(Task.CompletedTask);

        var result = await controller.DeleteAll();
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        mockService.Verify(s => s.DeleteAllAsync(), Times.Once);

    }
    
}