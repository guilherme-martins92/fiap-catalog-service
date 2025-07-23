using fiap_catalog_service.Models;

namespace fiap_catalog_service.Services
{
    public interface IVehicleService
    {
        /// <summary>
        /// Returns a list of all vehicles.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();

        /// <summary>
        /// Returns a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        Task<Vehicle?> GetVehicleByIdAsync(Guid id);

        /// <summary>
        /// Adds a new vehicle to the list.
        /// </summary>
        /// <param name="vehicle"></param>
        Task<Vehicle?> AddVehicleAsync(Vehicle vehicle);

        /// <summary>
        /// Updates an existing vehicle in the list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicle"></param>
        Task<Vehicle?> UpdateVehicleAsync(Guid id, Vehicle vehicle);

        Task<Vehicle?> ReserveVehicleAsync(Guid id);

        /// <summary>
        /// Deletes a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        Task<Vehicle?> DeleteVehicleAsync(Guid id);
    }
}
