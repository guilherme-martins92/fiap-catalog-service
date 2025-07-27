namespace fiap_reserve_vehicle_consumer.Models;

public class OrderServiceEvent
{
    public required string EventType { get; set; }     // "CompraCriada"
    public required ReserveVehicleDto ReserveVehicleDto { get; set; } // Contains VehicleId and OrderId
    public DateTime Timestamp { get; set; }
}
