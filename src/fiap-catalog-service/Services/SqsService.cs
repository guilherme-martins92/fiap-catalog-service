using Amazon.SQS;
using Amazon.SQS.Model;
using fiap_catalog_service.Models;
using System.Text.Json;

namespace fiap_catalog_service.Services
{
    public class SqsService : ISqsService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl = "http://localhost:4566/000000000000/VehicleEventsQueue";

        public SqsService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task SendMessageAsync(string eventType, Vehicle vehicle)
        {
            var message = new
            {
                EventType = eventType,
                Vehicle = vehicle
            };

            var request = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = JsonSerializer.Serialize(message)
            };

            await _sqsClient.SendMessageAsync(request);
        }
    }
}
