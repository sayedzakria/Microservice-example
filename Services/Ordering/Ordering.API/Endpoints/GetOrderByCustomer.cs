using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints;
//public record GetOrderByCustomerRequest(string CustomerName);
public record GetOrderByCustomerResponse(IEnumerable<OrderDto> Orders);
public class GetOrderByCustomer: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{id}", async (Guid id, ISender sender) =>
         {
             var result = await sender.Send(new GetOrdersByCustomerQuery(id));
             var response = result.Adapt<GetOrderByCustomerResponse>();
             return Results.Ok(response);
         })
         .WithName("GetOrderByCustomer")
         .Produces<GetOrderByCustomerResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Gets orders by customer name.")
         .WithSummary("Get orders by customer name");
    }
}