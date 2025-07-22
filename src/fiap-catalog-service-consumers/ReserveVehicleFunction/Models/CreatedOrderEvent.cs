namespace fiap_catalog_service_consumers.ReserveVehicleFunction.Models
{
    public class CreatedOrderEvent
    {
        public required string EventType { get; set; }     // "CompraCriada"
        public required string OrderId { get; set; }
        public required string VehicleId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}