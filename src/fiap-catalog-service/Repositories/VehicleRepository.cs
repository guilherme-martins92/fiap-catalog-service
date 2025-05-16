using Amazon.DynamoDBv2.DataModel;
using fiap_catalog_service.Models;

namespace fiap_catalog_service.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IDynamoDBContext _context;

        public VehicleRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.SaveAsync(vehicle);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.DeleteAsync<Vehicle>(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            var search = _context.ScanAsync<Vehicle>(null);
            var vehicles = new List<Vehicle>();
            do
            {
                var page = await search.GetNextSetAsync();
                vehicles.AddRange(page);
            } while (!search.IsDone);
            return vehicles;
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            var vehicle = await _context.LoadAsync<Vehicle>(id);
            return vehicle;
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            await _context.SaveAsync(vehicle);
        }
    }
}
