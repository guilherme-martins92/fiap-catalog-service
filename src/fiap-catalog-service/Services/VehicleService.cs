using fiap_catalog_service.Models;
using fiap_catalog_service.Repositories;

namespace fiap_catalog_service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Returns a list of all vehicles.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync() => await _vehicleRepository.GetAllAsync();

        /// <summary>
        /// Returns a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id) => await _vehicleRepository.GetByIdAsync(id);

        /// <summary>
        /// Adds a new vehicle to the list.
        /// </summary>
        /// <param name="vehicle"></param> 
        public async Task<Vehicle?> AddVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.AddAsync(vehicle);
            return vehicle;
        }

        /// <summary>
        /// Updates an existing vehicle in the list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicle"></param>
        public async Task<Vehicle?> UpdateVehicleAsync(Guid id, Vehicle vehicle)
        {
            var vehicleToUpdate = await _vehicleRepository.GetByIdAsync(id);
            if (vehicleToUpdate == null) return null;

            await _vehicleRepository.UpdateAsync(vehicle);
            return vehicle;
        }

        /// <summary>
        /// Deletes a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        public async Task<Vehicle?> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null) return null;

            await _vehicleRepository.DeleteAsync(id);
            return vehicle;
        }
    }
}
