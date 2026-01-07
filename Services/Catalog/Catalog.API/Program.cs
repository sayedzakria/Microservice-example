using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Get the current assembly to register MediatR handlers and validators.
var assembly = typeof(Program).Assembly;

// Configure MediatR
// - Registers handlers, requests, and notifications from the current assembly.
// - Adds pipeline behaviors for validation and logging.
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviors<,>)); // Adds validation behavior for requests.
    config.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Adds logging behavior for requests.
});

// Register FluentValidation validators from the current assembly.
builder.Services.AddValidatorsFromAssembly(assembly);

// Add Carter for modular endpoint composition.
builder.Services.AddCarter();

// Configure Marten (PostgreSQL-based document database)
// - Uses lightweight sessions for optimized read/write operations.
// - Connection string is retrieved from the configuration.
// - Seeds initial data in the development environment.
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

// Add a custom exception handler for centralized error handling.
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Configure health checks
// - Adds a PostgreSQL health check using the same database connection string.
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTP request pipeline

// Map Carter endpoints to the pipeline.
app.MapCarter();

// Add middleware for centralized exception handling.
app.UseExceptionHandler(options => { });

// Configure the health check endpoint
// - Exposes a /health endpoint.
// - Uses HealthChecks.UI to format the response for monitoring tools.
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Run the application.
app.Run();
