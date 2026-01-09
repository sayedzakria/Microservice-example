
namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketRequest(bool ISSuccess);
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeletBasketCommand(userName));
                var response = result.Adapt<DeleteBasketResult>();
                return Results.Ok(response);
            })
                .WithName("DeleteBasket")
                .WithTags("Basket Endpoints")
                .Produces<DeleteBasketRequest>(StatusCodes.Status200OK)
                .WithSummary("Deletes the basket for a specific user.")
                .WithDescription("Deletes the basket for a specific user by their username.");
        }
    }
}
