using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using fiap_catalog_service.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace fiap_catalog_service.Infrastructure.EventBridge
{
    [ExcludeFromCodeCoverage]
    public class EventBridgePublisher : IEventPublisher
    {
        private readonly IAmazonEventBridge _eventBridge;
        private readonly ILogger<EventBridgePublisher> _logger;
        private const string EventBusName = "saga-event-bus";

        public EventBridgePublisher(IAmazonEventBridge eventBridge, ILogger<EventBridgePublisher> logger)
        {
            _eventBridge = eventBridge;
            _logger = logger;
        }

        public async Task PublishVehicleReservedEventAsync(Guid orderId, Guid vehicleId)
        {
            var detail = JsonSerializer.Serialize(new
            {
                EventType = "VeiculoReservado",
                OrderId = orderId,
                VehicleId = vehicleId,
                Timestamp = DateTime.UtcNow
            });

            var request = new PutEventsRequest
            {
                Entries = new List<PutEventsRequestEntry>
            {
                new()
                {
                    Detail = detail,
                    DetailType = "VeiculoReservado",
                    Source = "ms.catalogo",
                    EventBusName = EventBusName
                }
            }
            };

            var response = await _eventBridge.PutEventsAsync(request);

            if (response.FailedEntryCount > 0)
            {
                throw new InvalidOperationException("Falha ao publicar evento CompraCancelada no EventBridge.");
            }
        }
    }
}