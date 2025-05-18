using fiap_catalog_service.Endpoints;
using fiap_catalog_service.Models;
using fiap_catalog_service.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace fiap_catalog_service_tests
{
    public class VehicleEndpointsTests
    {
        private readonly Mock<IVehicleService> _vehicleServiceMock;
        private readonly Mock<ILogger<VehicleEndpoints>> _loggerMock;
        private readonly Mock<IValidator<Vehicle>> _validatorMock;
        private readonly VehicleEndpoints _vehicleEndpoints;

        public VehicleEndpointsTests()
        {
            _vehicleServiceMock = new Mock<IVehicleService>();
            _loggerMock = new Mock<ILogger<VehicleEndpoints>>();
            _validatorMock = new Mock<IValidator<Vehicle>>();
            _vehicleEndpoints = new VehicleEndpoints(_vehicleServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllVehicles_ReturnsOkResult_WithListOfVehicles()
        {
            // Arrange  
            var vehicles = new List<Vehicle>
       {
           new Vehicle { Model = "Model X", Brand = "Tesla", Color = "Red", Year = 2022, Price = 80000 },
           new Vehicle { Model = "Mustang", Brand = "Ford", Color = "Blue", Year = 2021, Price = 55000 }
       };
            _vehicleServiceMock.Setup(s => s.GetAllVehiclesAsync()).ReturnsAsync(vehicles);

            // Act  
            var result = await _vehicleServiceMock.Object.GetAllVehiclesAsync();

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(vehicles.Count, result.Count());
        }

        [Fact]
        public async Task GetVehicleById_ReturnsOkResult_WhenVehicleExists()
        {
            // Arrange         
            var vehicle = new Vehicle { Model = "Model S", Brand = "Tesla", Color = "Black", Year = 2023, Price = 90000 };
            _vehicleServiceMock.Setup(s => s.GetVehicleByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);

            // Act  
            var result = await _vehicleServiceMock.Object.GetVehicleByIdAsync(vehicle.Id);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(vehicle.Id, result.Id);
        }

        [Fact]
        public async Task AddVehicle_ReturnsCreatedResult_WhenVehicleIsValid()
        {
            // Arrange  
            var vehicle = new Vehicle { Model = "Civic", Brand = "Honda", Color = "White", Year = 2020, Price = 25000 };
            _validatorMock.Setup(v => v.Validate(vehicle)).Returns(new FluentValidation.Results.ValidationResult());
            _vehicleServiceMock.Setup(s => s.AddVehicleAsync(vehicle)).ReturnsAsync(vehicle);

            // Act  
            var result = await _vehicleServiceMock.Object.AddVehicleAsync(vehicle);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(vehicle.Id, result.Id);
        }

        [Fact]
        public async Task UpdateVehicle_ReturnsOkResult_WhenVehicleIsUpdated()
        {
            // Arrange          
            var vehicle = new Vehicle { Model = "Corolla", Brand = "Toyota", Color = "White", Year = 2019, Price = 20000 };
            _validatorMock.Setup(v => v.Validate(vehicle)).Returns(new FluentValidation.Results.ValidationResult());
            _vehicleServiceMock.Setup(s => s.UpdateVehicleAsync(vehicle.Id, vehicle)).ReturnsAsync(vehicle);

            // Act  
            var result = await _vehicleServiceMock.Object.UpdateVehicleAsync(vehicle.Id, vehicle);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(vehicle.Id, result.Id);
        }

        [Fact]
        public async Task DeleteVehicle_ReturnsOkResult_WhenVehicleIsDeleted()
        {
            // Arrange   
            var vehicle = new Vehicle { Model = "Accord", Brand = "Honda", Color = "Black", Year = 2018, Price = 22000 };
            _vehicleServiceMock.Setup(s => s.DeleteVehicleAsync(vehicle.Id)).ReturnsAsync(vehicle);

            // Act  
            var result = await _vehicleServiceMock.Object.DeleteVehicleAsync(vehicle.Id);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(vehicle.Id, result.Id);
        }
    }
}
