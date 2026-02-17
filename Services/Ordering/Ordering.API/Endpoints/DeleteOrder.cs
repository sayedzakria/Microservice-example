namespace Ordering.API.Endpoints;

//public record DeleteOrderRequest(Guid Id);
public record DeleteOrderResponse(bool IsSuccess);
public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
         {
             var result = await sender.Send(new DeleteOrderCommand(id));
             var response = result.Adapt<DeleteOrderResponse>(); // Assuming the result is a boolean indicating success
             return Results.Ok(response);
         })
         .WithName("DeleteOrder")
         .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Deletes an existing order with the provided ID.")
         .WithSummary("Delete an existing order");
    }
}