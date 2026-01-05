var builder = WebApplication.CreateBuilder(args);
// Add services to container 
var app = builder.Build();

//configure the HTTP requests pipeline
app.Run();
