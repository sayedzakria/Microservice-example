var builder = WebApplication.CreateBuilder(args);
// Add services to container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
//Configure Marten with UseLightweightSessions "Best Practice" for read and write  and Posteger SQL Connections string 
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();

// configure the HTTP pipeline

app.MapCarter();
app.Run();
