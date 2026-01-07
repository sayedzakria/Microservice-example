
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShopingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName cannot be empty");
            RuleFor(x => x.Cart.Items).NotNull().WithMessage("Cart items cannot be null");
        }
    }
    public class StoreBasketCommandHandler:ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShopingCart cart = command.Cart;
            // Simulate storing the basket (e.g., in a database)
            await Task.Delay(100, cancellationToken); // Simulate async work
            // For demonstration, we assume the basket is always stored successfully
            return new StoreBasketResult("SWN");
        }
            
    }
}
