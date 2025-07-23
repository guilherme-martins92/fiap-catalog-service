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
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.OrderBy(v => v.Price);
        }

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

            vehicleToUpdate.Model = vehicle.Model;
            vehicleToUpdate.Brand = vehicle.Brand;
            vehicleToUpdate.Color = vehicle.Color;
            vehicleToUpdate.Year = vehicle.Year;
            vehicleToUpdate.Price = vehicle.Price;

            await _vehicleRepository.UpdateAsync(vehicleToUpdate);
            return vehicle;
        }

        public async Task<Vehicle?> ReserveVehicleAsync(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null) return null;
            if (vehicle.IsReserved)
            {
                throw new InvalidOperationException("Vehicle is already reserved.");
            }
            vehicle.IsReserved = true;
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