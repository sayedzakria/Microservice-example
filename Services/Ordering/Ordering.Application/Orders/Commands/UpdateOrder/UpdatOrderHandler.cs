


namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdatOrderHandler(IApplicationDbContext dbcontext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        //update order from command.Order
        var orderId = OrderId.Of(command.Order.Id);
        var order = await dbcontext.Orders.FindAsync([orderId], cancellationToken);
        if (order is null)
        {
            throw new OrderNotFoundException(command.Order.Id);
        }
        UpdateOrderWithNewValues(order, command.Order);
        //save changes to database
        dbcontext.Orders.Update(order);
        await dbcontext.SaveChangesAsync(cancellationToken);
        //return result
        return new UpdateOrderResult(true);
    }

    private static void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        var shippingAddress = Address.Of(
           orderDto.ShippingAddress.FirstName,
           orderDto.ShippingAddress.LastName,
           orderDto.ShippingAddress.EmailAddress,
           orderDto.ShippingAddress.AddressLine,
           orderDto.ShippingAddress.Country,
           orderDto.ShippingAddress.State,
           orderDto.ShippingAddress.ZipCode
       );
        var billingAddress = Address.Of(
             orderDto.BillingAddress.FirstName,
             orderDto.BillingAddress.LastName,
             orderDto.BillingAddress.EmailAddress,
             orderDto.BillingAddress.AddressLine,
             orderDto.BillingAddress.Country,
             orderDto.BillingAddress.State,
             orderDto.BillingAddress.ZipCode
         );
        var payment = Payment.Of(
            orderDto.Payment.CardName,
            orderDto.Payment.CardNumber,
            orderDto.Payment.Expiration,
            orderDto.Payment.Cvv,
            orderDto.Payment.PaymentMethod
        );
       order.updateorder(
            OrderName.Of(orderDto.OrderName),
            shippingAddress,
            billingAddress,
            payment,
            orderDto.Status
        );
    }
}
