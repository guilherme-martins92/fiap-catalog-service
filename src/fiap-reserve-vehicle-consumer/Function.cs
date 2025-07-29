using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using fiap_reserve_vehicle_consumer.Models;
using System.Text;
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
                        await ReserveVehicleAsync(orderEvent.ReserveVehicleDto, context);

                    if (orderEvent.EventType == "CompraCancelada" || orderEvent.EventType == "PagamentoNaoRealizado")
                        await UnreserveVehicleAsync(orderEvent.ReserveVehicleDto, context);
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error processing record: {ex.Message}");
                throw;
            }
        }
    }

    private async Task ReserveVehicleAsync(ReserveVehicleDto reserveVehicleDto, ILambdaContext context)
    {
        var json = JsonSerializer.Serialize(reserveVehicleDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync("https://qck4zlo8gl.execute-api.us-east-1.amazonaws.com/vehicles/reserve", content);

        if (response.IsSuccessStatusCode)
        {
            context.Logger.LogLine($"Vehicle {reserveVehicleDto.VehicleId} reserved successfully.");
        }
        else
        {
            context.Logger.LogLine($"Failed to reserve vehicle {reserveVehicleDto.VehicleId}. Status code: {response.StatusCode}");
        }
    }

    private async Task UnreserveVehicleAsync(ReserveVehicleDto reserveVehicleDto, ILambdaContext context)
    {
        var json = JsonSerializer.Serialize(reserveVehicleDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"https://qck4zlo8gl.execute-api.us-east-1.amazonaws.com/vehicles/unreserve/", content);

        if (response.IsSuccessStatusCode)
        {
            context.Logger.LogLine($"Vehicle {reserveVehicleDto.VehicleId} unreserved successfully.");
        }
        else
        {
            context.Logger.LogLine($"Failed to unreserve vehicle {reserveVehicleDto.VehicleId}. Status code: {response.StatusCode}");
        }
    }
}