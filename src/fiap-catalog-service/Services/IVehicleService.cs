using fiap_catalog_service.Models;

namespace fiap_catalog_service.Services
{
    public interface IVehicleService
    {
        /// <summary>
        /// Returns a list of all vehicles.
        /// </summary>
        /// <returns></returns>
        List<Vehicle> GetVehicles();

        /// <summary>
        /// Returns a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        Vehicle? GetVehicleById(Guid id);

        /// <summary>
        /// Adds a new vehicle to the list.
        /// </summary>
        /// <param name="vehicle"></param>
        Vehicle AddVehicle(Vehicle vehicle);

        /// <summary>
        /// Updates an existing vehicle in the list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicle"></param>
        Vehicle? UpdateVehicle(Guid id, Vehicle vehicle);
    }
}
