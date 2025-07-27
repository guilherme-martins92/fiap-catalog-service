namespace fiap_catalog_service.Infrastructure.EventBridge
{
    public interface IEventPublisher
    {
        Task PublishVehicleReservedEventAsync(Guid orderId, Guid vehicleId);
    }
}