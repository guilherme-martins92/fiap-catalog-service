using fiap_catalog_service.Models;
using fiap_catalog_service.Services;
using FluentValidation;

namespace fiap_catalog_service.Endpoints
{
    public class VehicleEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            var vehicleService = new VehicleService();

            // GET: Retorna todos os veículos    
            app.MapGet("/vehicles", () => vehicleService.GetVehicles());

            // GET: Busca um veículo por ID    
            app.MapGet("/vehicles/{id}", (Guid id) => vehicleService.GetVehicleById(id) is Models.Vehicle vehicle ? Results.Ok(vehicle) : Results.NotFound());

            // POST: Cadastra um novo veículo 
            app.MapPost("/vehicles", (Vehicle vehicle, IValidator<Vehicle> validator) =>
            {
                var result = validator.Validate(vehicle);

                if (!result.IsValid)
                {
                    var errors = result.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
                    return Results.ValidationProblem(errors);
                }

                return Results.Created($"/cars/{vehicle.Id}", vehicle);
            });

            // PUT: Atualiza um veículo existente     
            app.MapPut("/vehicles/{id}", (Guid id, Vehicle vehicle, IValidator<Vehicle> validator) =>
            {
                var result = validator.Validate(vehicle);

                if (!result.IsValid)
                {
                    var errors = result.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
                    return Results.ValidationProblem(errors);
                }

                var updatedCar = vehicleService.UpdateVehicle(id, vehicle);
                return updatedCar is not null ? Results.Ok(updatedCar) : Results.NotFound();
            });
        }
    }
}
