

var builder = WebApplication.CreateBuilder(args);
// Add services to container
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviors<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();

//Configure Marten with UseLightweightSessions "Best Practice" for read and write  and Posteger SQL Connections string 
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

// configure the HTTP pipeline

app.MapCarter();
app.UseExceptionHandler(options => { });

app.Run();
