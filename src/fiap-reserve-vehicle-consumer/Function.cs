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
                // Deserialize the message body to get the vehicle ID
                var envelope = JsonSerializer.Deserialize<JsonElement>(record.Body);

                if (envelope.TryGetProperty("detail", out var detailJson))
                {
                    var createdOrderEvent = JsonSerializer.Deserialize<CreatedOrderEvent>(detailJson);

                    if (createdOrderEvent == null)
                    {
                        context.Logger.LogLine("Failed to deserialize message body.");
                        continue;
                    }

                    // Call the catalog service to reserve the vehicle
                    var response = await client.PutAsync($"https://qck4zlo8gl.execute-api.us-east-1.amazonaws.com/vehicles/reserve/{createdOrderEvent.VehicleId}", null);
                    if (response.IsSuccessStatusCode)
                    {
                        context.Logger.LogLine($"Vehicle {createdOrderEvent.VehicleId} reserved successfully.");
                    }
                    else
                    {
                        context.Logger.LogLine($"Failed to reserve vehicle {createdOrderEvent.VehicleId}. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error processing record: {ex.Message}");
                throw;
            }
        }
    }
}