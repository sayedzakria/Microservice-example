using Discount.Grpc;

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
    public class StoreBasketCommandHandler(IBsaketRepository repository, DiscountService.DiscountServiceClient discountProto) 
        :ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShopingCart cart = command.Cart;
          await  DeductDiscountsFromCartItems(cart,cancellationToken);
            
            await repository.StoreBasket(cart, cancellationToken);
            return new StoreBasketResult(cart.UserName);
        }
         
        private async Task DeductDiscountsFromCartItems(ShopingCart cart,CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
                item.Price -= (decimal)coupon.Amount; // Fix: cast double to decimal
            }
        }
    }
}
