using BuildingBlocks.Behaviors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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
var app = builder.Build();

// configure the HTTP pipeline

app.MapCarter();
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if(exception==null)
            { return; }
        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace
        };
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception,exception.Message);
        context.Response.StatusCode=StatusCodes.Status500InternalServerError;
        context.Response.ContentType="application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    });

});
app.Run();
