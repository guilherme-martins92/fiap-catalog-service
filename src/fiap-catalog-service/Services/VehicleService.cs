using fiap_catalog_service.Models;

namespace fiap_catalog_service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly List<Vehicle> _vehicles = new();

        /// <summary>
        /// Returns a list of all vehicles.
        /// </summary>
        /// <returns></returns>
        public List<Vehicle> GetVehicles() => _vehicles;

        /// <summary>
        /// Returns a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        public Vehicle? GetVehicleById(Guid id) => _vehicles.FirstOrDefault(v => v.Id == id);

        /// <summary>
        /// Adds a new vehicle to the list.
        /// </summary>
        /// <param name="vehicle"></param> 
        public Vehicle AddVehicle(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
            return vehicle;
        }

        /// <summary>
        /// Updates an existing vehicle in the list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicle"></param>
        public Vehicle? UpdateVehicle(Guid id, Vehicle vehicle)
        {
            var existingVehicle = _vehicles.FirstOrDefault(v => v.Id == id);
            if (existingVehicle == null) return null;

            _vehicles.Remove(existingVehicle);
            _vehicles.Add(vehicle);
            return vehicle;
        }

        /// <summary>
        /// Deletes a vehicle by its ID.
        /// </summary>
        /// <param name="id"></param>
        public Vehicle? DeleteVehicle(Guid id)
        {
            var vehicle = _vehicles.FirstOrDefault(c => c.Id == id);
            if (vehicle == null) return null;

            _vehicles.Remove(vehicle);
            return vehicle;
        }
    }
}
