using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using fiap_reserve_vehicle_consumer.Models;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ReserveVehicle;

public class Function
{

    private readonly HttpClient client = new HttpClient();

    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        foreach (var record in sqsEvent.Records)
        {
            try
            {
                context.Logger.LogLine($"Processing message ID: {record.MessageId}");

                var envelope = JsonSerializer.Deserialize<JsonElement>(record.Body);

                if (envelope.TryGetProperty("detail", out var detailJson))
                {
                    var orderEvent = JsonSerializer.Deserialize<OrderServiceEvent>(detailJson);

                    if (orderEvent == null)
                    {
                        context.Logger.LogLine("Failed to deserialize message body.");
                        continue;
                    }

                    if (orderEvent.EventType == "CompraRealizada")
                        await ReserveVehicleAsync(orderEvent.VehicleId, context);

                    if (orderEvent.EventType == "CompraCancelada")
                        await UnreserveVehicleAsync(orderEvent.VehicleId, context);

                    if (orderEvent.EventType == "PagamentoNaoRealizado")
                        await UnreserveVehicleAsync(orderEvent.VehicleId, context);
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error processing record: {ex.Message}");
                throw;
            }
        }
    }

    private async Task ReserveVehicleAsync(string vehicleId, ILambdaContext context)
    {
        var response = await client.PutAsync($"https://qck4zlo8gl.execute-api.us-east-1.amazonaws.com/vehicles/reserve/{vehicleId}", null);

        if (response.IsSuccessStatusCode)
        {
            context.Logger.LogLine($"Vehicle {vehicleId} reserved successfully.");
        }
        else
        {
            context.Logger.LogLine($"Failed to reserve vehicle {vehicleId}. Status code: {response.StatusCode}");
        }
    }

    private async Task UnreserveVehicleAsync(string vehicleId, ILambdaContext context)
    {
        var response = await client.PutAsync($"https://qck4zlo8gl.execute-api.us-east-1.amazonaws.com/vehicles/unreserve/{vehicleId}", null);

        if (response.IsSuccessStatusCode)
        {
            context.Logger.LogLine($"Vehicle {vehicleId} unreserved successfully.");
        }
        else
        {
            context.Logger.LogLine($"Failed to unreserve vehicle {vehicleId}. Status code: {response.StatusCode}");
        }
    }
}