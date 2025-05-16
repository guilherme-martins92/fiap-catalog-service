using System.ComponentModel.DataAnnotations;

namespace fiap_catalog_service.Models
{
    public class Vehicle
    {
        /// <summary>
        /// Unique identifier for the vehicle.
        /// </summary>
        public Guid Id { get; set; }

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
    }
}
