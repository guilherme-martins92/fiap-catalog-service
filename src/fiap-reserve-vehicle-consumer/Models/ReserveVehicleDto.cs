namespace fiap_reserve_vehicle_consumer.Models
{
    public class ReserveVehicleDto
    {
        public required Guid VehicleId { get; init; }
        public required Guid OrderId { get; init; }
    }
}
