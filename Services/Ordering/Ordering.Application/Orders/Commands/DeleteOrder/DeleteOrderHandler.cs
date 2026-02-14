
namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        //Delete order entity from database using command.OrderId
        var orderId = OrderId.Of(command.OrderId);
        var order = await dbContext.Orders.FindAsync(orderId, cancellationToken);
        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }
        //save changes to database
                dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        //return result indicating success or failure
        return new DeleteOrderResult (true);
    }
}
