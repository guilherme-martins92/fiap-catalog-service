using fiap_catalog_service.Endpoints;
using fiap_catalog_service.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<VehicleValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

VehicleEndpoints.RegisterEndpoints(app);

app.Run();
