using fiap_catalog_service.Models;
using fiap_catalog_service.Services;
using FluentValidation;

namespace fiap_catalog_service.Endpoints
{
    public class VehicleEndpoints
    {
        private readonly IVehicleService _vehicleService;

        public VehicleEndpoints(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public void RegisterEndpoints(WebApplication app)
        {
            // GET: Retorna todos os veículos      
            app.MapGet("/vehicles", async () => await _vehicleService.GetAllVehiclesAsync());

            // GET: Busca um veículo por ID      
            app.MapGet("/vehicles/{id}", async (Guid id) => await _vehicleService.GetVehicleByIdAsync(id) is Vehicle vehicle ? Results.Ok(vehicle) : Results.NotFound());

            // POST: Cadastra um novo veículo   
            app.MapPost("/vehicles", async (Vehicle vehicle, IValidator<Vehicle> validator) =>
            {
                var result = validator.Validate(vehicle);

                if (!result.IsValid)
                {
                    var errors = result.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
                    return Results.ValidationProblem(errors);
                }

                var createdVehicle = await _vehicleService.AddVehicleAsync(vehicle);
                return Results.Created($"/vehicles/{createdVehicle.Id}", createdVehicle);
            });

            // PUT: Atualiza um veículo existente       
            app.MapPut("/vehicles/{id}", async (Guid id, Vehicle vehicle, IValidator<Vehicle> validator) =>
            {
                var result = validator.Validate(vehicle);

                if (!result.IsValid)
                {
                    var errors = result.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
                    return Results.ValidationProblem(errors);
                }

                var updatedCar = await _vehicleService.UpdateVehicleAsync(id, vehicle);
                return updatedCar is not null ? Results.Ok(updatedCar) : Results.NotFound();
            });

            // DELETE: Remove um veículo existente  
            app.MapDelete("/vehicles/{id}", async (Guid id) =>
            {
                var deletedVehicle = await _vehicleService.DeleteVehicleAsync(id);
                return deletedVehicle is not null ? Results.Ok(deletedVehicle) : Results.NotFound();
            });
        }
    }
}
