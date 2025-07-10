using UParkFoodtruckAPI;
using UParkFoodtruckAPI.Endpoints;
using UParkFoodtruckAPI.Repositories;
using UParkFoodtruckAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Services
builder.Services.AddEndpointsApiExplorer(); // Needed for minimal API
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    //options.SchemaFilter<EnumSchemaFilter>();
});

builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapReservationEndpoints();
app.UseHttpsRedirection();
app.Run();
