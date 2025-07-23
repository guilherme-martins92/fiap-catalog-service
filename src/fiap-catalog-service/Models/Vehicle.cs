using Amazon.DynamoDBv2.DataModel;

namespace fiap_catalog_service.Models
{
    [DynamoDBTable("Vehicles")]
    public class Vehicle
    {
        /// <summary>
        /// Unique identifier for the vehicle.
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// The model of the vehicle.
        /// </summary>
        public required string Model { get; set; }

        /// <summary>
        /// The brand of the vehicle.
        /// </summary>
        public required string Brand { get; set; }

        /// <summary>
        /// The color of the vehicle.
        /// </summary>
        public required string Color { get; set; }

        /// <summary>
        /// The year the vehicle was manufactured.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The Price of the vehicle.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is reserved.
        /// </summary>
        public bool IsReserved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is available.
        /// </summary>
        public bool IsAvailable { get; set; }

    }
}
