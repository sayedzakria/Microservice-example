

namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //create order entity from command object
        //save order entity to database
        //return order id as result
        throw new NotImplementedException();
    }
}
