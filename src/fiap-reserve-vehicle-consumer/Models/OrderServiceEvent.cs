namespace fiap_reserve_vehicle_consumer.Models;

public class OrderServiceEvent
{
    public required string EventType { get; set; }     // "CompraCriada"
    public required string OrderId { get; set; }
    public required string VehicleId { get; set; }
    public DateTime Timestamp { get; set; }
}
