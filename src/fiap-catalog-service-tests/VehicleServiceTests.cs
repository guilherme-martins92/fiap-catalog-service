using fiap_catalog_service.Dtos;
using fiap_catalog_service.Infrastructure.EventBridge;
using fiap_catalog_service.Models;
using fiap_catalog_service.Repositories;
using fiap_catalog_service.Services;
using Moq;

namespace fiap_catalog_service_tests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _mockRepository;
        private readonly Mock<IEventPublisher> _mockEventPublisher;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTests()
        {
            _mockRepository = new Mock<IVehicleRepository>();
            _mockEventPublisher = new Mock<IEventPublisher>();
            _vehicleService = new VehicleService(_mockRepository.Object, _mockEventPublisher.Object);            
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
        [Fact]
        public async Task ReserveVehicleAsync_ShouldReserveVehicle_WhenVehicleIsNotReserved()
        {
            // Arrange     
            var orderId = Guid.NewGuid();

            var vehicle = new Vehicle
            {
                Model = "Model 3",
                Brand = "Tesla",
                Color = "White",
                Year = 2021,
                Price = 35000,
                IsReserved = false,
                IsAvailable = true
            };

            var reserveVehicleDto = new ReserveVehicleDto
            {
                VehicleId = vehicle.Id,
                OrderId = orderId
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(reserveVehicleDto.VehicleId)).ReturnsAsync(vehicle);
            _mockRepository.Setup(repo => repo.UpdateAsync(vehicle)).Returns(Task.CompletedTask);
            _mockEventPublisher.Setup(publisher => publisher.PublishVehicleReservedEventAsync(orderId, vehicle.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _vehicleService.ReserveVehicleAsync(reserveVehicleDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(vehicle.IsReserved);
            Assert.False(vehicle.IsAvailable);
            _mockRepository.Verify(repo => repo.UpdateAsync(vehicle), Times.Once);
            _mockEventPublisher.Verify(publisher => publisher.PublishVehicleReservedEventAsync(orderId, vehicle.Id), Times.Once);
        }

        [Fact]
        public async Task ReserveVehicleAsync_ShouldReturnNull_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var reserveVehicleDto = new ReserveVehicleDto
            {
                VehicleId = vehicleId,
                OrderId = orderId
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync((Vehicle?)null);

            // Act
            var result = await _vehicleService.ReserveVehicleAsync(reserveVehicleDto);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task ReserveVehicleAsync_ShouldThrowException_WhenVehicleIsAlreadyReserved()
        {
            var vehicleId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var vehicle = new Vehicle
            {
                Model = "Model Y",
                Brand = "Tesla",
                Color = "Black",
                Year = 2022,
                Price = 60000,
                IsReserved = true,
                IsAvailable = false
            };

            var reserveVehicleDto = new ReserveVehicleDto
            {
                VehicleId = vehicleId,
                OrderId = orderId
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);
            _mockRepository.Setup(repo => repo.UpdateAsync(vehicle)).Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _vehicleService.ReserveVehicleAsync(reserveVehicleDto));
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task UnreserveVehicleAsync_ShouldUnreserveVehicle_WhenVehicleIsReserved()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle
            {
                Model = "Focus",
                Brand = "Ford",
                Color = "Red",
                Year = 2021,
                Price = 20000,
                IsReserved = true
            };
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);
            _mockRepository.Setup(repo => repo.UpdateAsync(vehicle)).Returns(Task.CompletedTask);
            // Act
            var result = await _vehicleService.UnreserveVehicleAsync(vehicleId);
            // Assert
            Assert.NotNull(result);
            Assert.False(vehicle.IsReserved);
            Assert.True(vehicle.IsAvailable);
            _mockRepository.Verify(repo => repo.UpdateAsync(vehicle), Times.Once);
        }

        [Fact]
        public async Task UnreserveVehicleAsync_ShouldReturnNull_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync((Vehicle?)null);
            // Act
            var result = await _vehicleService.UnreserveVehicleAsync(vehicleId);
            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task UnreserveVehicleAsync_ShouldThrowException_WhenVehicleIsNotReserved()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle
            {
                Model = "Camry",
                Brand = "Toyota",
                Color = "Silver",
                Year = 2019,
                Price = 23000,
                IsReserved = false
            };
            _mockRepository.Setup(repo => repo.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _vehicleService.UnreserveVehicleAsync(vehicleId));
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Vehicle>()), Times.Never);
        }
    }
}