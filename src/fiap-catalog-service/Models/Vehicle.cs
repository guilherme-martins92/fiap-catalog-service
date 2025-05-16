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
        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O modelo deve ter no máximo 100 caracteres.")]
        public required string Model { get; set; }

        /// <summary>
        /// The brand of the vehicle.
        /// </summary>
        [Required(ErrorMessage = "A marca é obrigatória.")]
        [StringLength(100, ErrorMessage = "A marca deve ter no máximo 100 caracteres.")]
        public required string Brand { get; set; }

        /// <summary>
        /// The color of the vehicle.
        /// </summary>
        [Required(ErrorMessage = "A cor é obrigatória.")]
        [StringLength(50, ErrorMessage = "A cor deve ter no máximo 50 caracteres.")]
        public required string Color { get; set; }

        /// <summary>
        /// The year the vehicle was manufactured.
        /// </summary>
        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(1990, 2100, ErrorMessage = "O ano deve estar entre 1990 e 2100.")]
        public int Year { get; set; }

        /// <summary>
        /// The Price of the vehicle.
        /// </summary>
        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }
    }
}
