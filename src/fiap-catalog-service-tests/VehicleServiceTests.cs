using fiap_catalog_service.Models;
using fiap_catalog_service.Repositories;
using fiap_catalog_service.Services;
using Moq;

namespace fiap_catalog_service_tests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _mockRepository;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTests()
        {
            _mockRepository = new Mock<IVehicleRepository>();
            _vehicleService = new VehicleService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllVehiclesAsync_ShouldReturnAllVehicles()
        {
            // Arrange  
            var vehicles = new List<Vehicle>
            {
                new Vehicle { Model = "Civic", Brand = "Honda", Color = "Blue", Year = 2020, Price = 20000 },
                new Vehicle { Model = "Model X", Brand = "Tesla", Color = "Red", Year = 2022, Price = 80000 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(vehicles);

            // Act  
            var result = await _vehicleService.GetAllVehiclesAsync();

            // Assert  
            Assert.Equal(vehicles, result);
        }

        [Fact]
        public async Task GetVehicleByIdAsync_ShouldReturnVehicle_WhenVehicleExists()
        {
            // Arrange  
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle { Model = "Model S", Brand = "Tesla", Color = "Black", Year = 2021, Price = 75000 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);

            // Act  
            var result = await _vehicleService.GetVehicleByIdAsync(vehicleId);

            // Assert  
            Assert.Equal(vehicle, result);
        }

        [Fact]
        public async Task GetVehicleByIdAsync_ShouldReturnNull_WhenVehicleDoesNotExist()
        {
            // Arrange  
            var vehicleId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync((Vehicle?)null);

            // Act  
            var result = await _vehicleService.GetVehicleByIdAsync(vehicleId);

            // Assert  
            Assert.Null(result);
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldAddVehicle()
        {
            // Arrange  
            var vehicle = new Vehicle { Model = "Corolla", Brand = "Toyota", Color = "White", Year = 2019, Price = 18000 };
            _mockRepository.Setup(repo => repo.AddAsync(vehicle)).Returns(Task.CompletedTask);

            // Act  
            var result = await _vehicleService.AddVehicleAsync(vehicle);

            // Assert  
            Assert.Equal(vehicle, result);
            _mockRepository.Verify(repo => repo.AddAsync(vehicle), Times.Once);
        }

        [Fact]
        public async Task UpdateVehicleAsync_ShouldUpdateVehicle_WhenVehicleExists()
        {
            // Arrange  
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle { Model = "Accord", Brand = "Honda", Color = "Gray", Year = 2018, Price = 22000 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);
            _mockRepository.Setup(repo => repo.UpdateAsync(vehicle)).Returns(Task.CompletedTask);

            // Act  
            var result = await _vehicleService.UpdateVehicleAsync(vehicleId, vehicle);

            // Assert  
            Assert.Equal(vehicle, result);
            _mockRepository.Verify(repo => repo.UpdateAsync(vehicle), Times.Once);
        }

        [Fact]
        public async Task UpdateVehicleAsync_ShouldReturnNull_WhenVehicleDoesNotExist()
        {
            // Arrange  
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle { Model = "Camry", Brand = "Toyota", Color = "Silver", Year = 2017, Price = 20000 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync((Vehicle?)null);
            _mockRepository.Setup(repo => repo.UpdateAsync(vehicle)).Returns(Task.CompletedTask);



            // Act  
            var result = await _vehicleService.UpdateVehicleAsync(vehicleId, vehicle);

            // Assert  
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task DeleteVehicleAsync_ShouldDeleteVehicle_WhenVehicleExists()
        {
            // Arrange  
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle { Model = "Mustang", Brand = "Ford", Color = "Yellow", Year = 2021, Price = 55000 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);
            _mockRepository.Setup(repo => repo.DeleteAsync(vehicleId)).Returns(Task.CompletedTask);

            // Act  
            var result = await _vehicleService.DeleteVehicleAsync(vehicleId);

            // Assert  
            Assert.Equal(vehicle, result);
            _mockRepository.Verify(repo => repo.DeleteAsync(vehicleId), Times.Once);  
        }

        [Fact]
        public async Task DeleteVehicleAsync_ShouldReturnNull_WhenVehicleDoesNotExist()
        {
            // Arrange  
            var vehicleId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync((Vehicle?)null);
            _mockRepository.Setup(repo => repo.DeleteAsync(vehicleId)).Returns(Task.CompletedTask);

            // Act  
            var result = await _vehicleService.DeleteVehicleAsync(vehicleId);

            // Assert  
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never); 
        }
    }
}