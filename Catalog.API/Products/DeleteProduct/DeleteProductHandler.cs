
namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id):ICommand<DeletProductResult>;
    public record DeletProductResult(bool IsSucsess);
    internal class DeleteProductCommandHandler(IDocumentSession session,ILogger<DeleteProductCommandHandler>logger)
        : ICommandHandler<DeleteProductCommand, DeletProductResult>
    {
        public async Task<DeletProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler.Handle called with {@command}", command);
            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);
            return new DeletProductResult(true);

        }
    }
}
