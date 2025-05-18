using fiap_catalog_service.Constants;
using fiap_catalog_service.Models;
using fiap_catalog_service.Repositories;

namespace fiap_catalog_service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ISqsService _sqsService;

        public VehicleService(IVehicleRepository vehicleRepository, ISqsService sqsService)
        {
            _vehicleRepository = vehicleRepository;
            _sqsService = sqsService;
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
            await _sqsService.SendMessageAsync(VehicleEventType.Created, vehicle);
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
            await _sqsService.SendMessageAsync(VehicleEventType.Updated, vehicle);
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
            await _sqsService.SendMessageAsync(VehicleEventType.Deleted, vehicle);
            return vehicle;
        }
    }
}
