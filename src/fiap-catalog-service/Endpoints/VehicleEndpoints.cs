using fiap_catalog_service.Models;
using fiap_catalog_service.Services;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace fiap_catalog_service.Endpoints
{
    [ExcludeFromCodeCoverage]
    public class VehicleEndpoints
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehicleEndpoints> _logger;


        public VehicleEndpoints(IVehicleService vehicleService, ILogger<VehicleEndpoints> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        public void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/vehicles", async () =>
            {
                try
                {
                    _logger.LogInformation("Buscando todos os veículos");

                    var vehicles = await _vehicleService.GetAllVehiclesAsync();
                    return Results.Ok(vehicles);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao buscar veículos");
                    return Results.Problem(title: "Erro interno");
                }
            });

            app.MapGet("/vehicles/{id}", async (Guid id) =>
            {
                try
                {
                    _logger.LogInformation("Buscando veículo com ID: {Id}", id);

                    var vehicle = await _vehicleService.GetVehicleByIdAsync(id);

                    return vehicle is not null
                        ? Results.Ok(vehicle)
                        : Results.NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao buscar veículo com ID: {Id}", id);
                    return Results.Problem(title: "Erro interno");
                }
            });

            app.MapPost("/vehicles", async (Vehicle vehicle, IValidator<Vehicle> validator) =>
            {
                try
                {
                    _logger.LogInformation("Cadastrando veículo: {Vehicle}", JsonSerializer.Serialize(vehicle));

                    var result = validator.Validate(vehicle);

                    if (!result.IsValid)
                    {
                        var errors = result.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                        return Results.ValidationProblem(errors);
                    }

                    var createdVehicle = await _vehicleService.AddVehicleAsync(vehicle);

                    if (createdVehicle is null)
                        throw new Exception();

                    return Results.Created($"/vehicles/{createdVehicle.Id}", createdVehicle);
                }
                catch (ValidationException ex)
                {
                    return Results.BadRequest(new { Message = "Erro de validação", ex.Errors });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao cadastrar veículo: {Vehicle}", JsonSerializer.Serialize(vehicle));
                    return Results.Problem(title: "Erro interno");
                }
            });

            app.MapPut("/vehicles/{id}", async (Guid id, Vehicle vehicle, IValidator<Vehicle> validator) =>
            {
                try
                {
                    _logger.LogInformation("Atualizando veículo com ID: {Id}", id);

                    var result = validator.Validate(vehicle);

                    if (!result.IsValid)
                    {
                        var errors = result.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
                        return Results.ValidationProblem(errors);
                    }

                    var updatedCar = await _vehicleService.UpdateVehicleAsync(id, vehicle);
                    return updatedCar is not null ? Results.Ok(updatedCar) : Results.NotFound();
                }
                catch (ValidationException ex)
                {
                    return Results.BadRequest(new { Message = "Erro de validação", ex.Errors });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar veículo com ID: {Id}", id);
                    return Results.Problem(title: "Erro interno");
                }
            });

            app.MapDelete("/vehicles/{id}", async (Guid id) =>
            {
                try
                {
                    _logger.LogInformation("Removendo veículo com ID: {Id}", id);

                    var deletedVehicle = await _vehicleService.DeleteVehicleAsync(id);

                    return deletedVehicle is not null
                        ? Results.Ok(deletedVehicle)
                        : Results.NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao remover veículo com ID: {Id}", id);

                    return Results.Problem(title: "Erro interno", detail: ex.Message);
                }
            });
        }
    }
}
