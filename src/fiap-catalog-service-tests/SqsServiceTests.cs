using Amazon.SQS;
using Amazon.SQS.Model;
using fiap_catalog_service.Models;
using fiap_catalog_service.Services;
using Moq;

namespace fiap_catalog_service_tests
{
    public class SqsServiceTests
    {
        private readonly Mock<IAmazonSQS> _mockSqsClient;
        private readonly SqsService _sqsService;

        public SqsServiceTests()
        {
            _mockSqsClient = new Mock<IAmazonSQS>();
            _sqsService = new SqsService(_mockSqsClient.Object);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendCorrectMessage()
        {
            var eventType = "VehicleCreated";
            var vehicle = new Vehicle
            {
                Model = "Model S",
                Brand = "Tesla",
                Color = "Red",
                Year = 2023,
                Price = 79999.99m
            };

            _mockSqsClient.Setup(client => client.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ReturnsAsync(new SendMessageResponse());

            // Act
            await _sqsService.SendMessageAsync(eventType, vehicle);

            // Assert
            _mockSqsClient.Verify(client => client.SendMessageAsync(It.IsAny<SendMessageRequest>(), default), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldThrowException_WhenSqsClientFails()
        {
            // Arrange
            var eventType = "VehicleCreated";
            var vehicle = new Vehicle
            {
                Model = "Model S",
                Brand = "Tesla",
                Color = "Red",
                Year = 2023,
                Price = 79999.99m
            };

            _mockSqsClient
                .Setup(client => client.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ThrowsAsync(new Exception("SQS error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _sqsService.SendMessageAsync(eventType, vehicle));
        }
    }
}
