using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.API.Endpoints;

//public record GetOrderByNameRequest(string Name);
public record GetOrderByNameResponse(IEnumerable< OrderDto> Orders);
public class GetOrderByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{name}", async (string name, ISender sender) =>
         {
             var result = await sender.Send(new GetOrderByNameQuery(name));
             var response = result.Adapt<GetOrderByNameResponse>();
             return Results.Ok(response);
         })
         .WithName("GetOrderByName")
         .Produces<GetOrderByNameResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Gets an order by its name.")
         .WithSummary("Get an order by name");
    }
}