



namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>; 

public record CreateOrderResult(Guid OrderId);
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order name is required.");
        RuleFor(x => x.Order.CusromerId).NotNull().WithMessage("Customer ID is required.");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order must have at least one item." );
    }
}