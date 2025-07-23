using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using fiap_catalog_service.Repositories;
using fiap_catalog_service_consumers.ReserveVehicleFunction.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace catalog.Reserve;

[ExcludeFromCodeCoverage]
public class Function
{
    private readonly VehicleRepository _vehicleRepository;

    public Function()
    {
        var client = new AmazonDynamoDBClient(); // usa config da Lambda
        var context = new DynamoDBContext(client);
        _vehicleRepository = new VehicleRepository(context);
    }

    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        foreach (var message in sqsEvent.Records)
        {
            var evento = JsonSerializer.Deserialize<CreatedOrderEvent>(message.Body);

            if (evento is not null)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(Guid.Parse(evento.VehicleId));

                if (vehicle is not null)
                {
                    vehicle.Available = false;
                    vehicle.Reserved = true;
                    await _vehicleRepository.UpdateAsync(vehicle);
            
                    context.Logger.LogLine($"Veículo {vehicle.Id} reservado com sucesso para compra {evento.OrderId}.");
                }
                else
                {
                    context.Logger.LogLine($"Veículo com ID {evento.VehicleId} não encontrado.");
                }
            }
        }
    }
}