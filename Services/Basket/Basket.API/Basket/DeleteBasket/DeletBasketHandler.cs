
namespace Basket.API.Basket.DeleteBasket
{
    public record DeletBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);
    public class DeletBasketCommandValidator : AbstractValidator<DeletBasketCommand>
    {
        public DeletBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.");
        }
    }
    public class DeletBasketCommandHandler : ICommandHandler<DeletBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeletBasketCommand command, CancellationToken cancellationToken)
        {
            //TODO: Implement the handler logic to delete the basket for the given UserName.

            return new DeleteBasketResult(true);
        }
    }
}
