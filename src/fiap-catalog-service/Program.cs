using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using fiap_catalog_service.Endpoints;
using fiap_catalog_service.Repositories;
using fiap_catalog_service.Validators;
using FluentValidation;
using fiap_catalog_service.Services;
using Amazon.SQS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<VehicleValidator>();

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    return new AmazonDynamoDBClient(new AmazonDynamoDBConfig
    {
        ServiceURL = "http://localhost:4566"
    });
});

builder.Services.AddSingleton<IAmazonSQS>(sp =>
{
    return new AmazonSQSClient(new AmazonSQSConfig
    {
        ServiceURL = "http://localhost:4566"
    });
});

builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ISqsService, SqsService>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<VehicleEndpoints>>();

    var vehicleEndpoints = new VehicleEndpoints(vehicleService, logger);
    vehicleEndpoints.RegisterEndpoints(app);
}

app.Run();
