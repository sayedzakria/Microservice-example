

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<String> Category, string Description, string ImageFile, decimal Price) :
        ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateProductCommandtHandler(IDocumentSession documentSession)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create product entity from command
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };
            //TODO
            //save to database
            documentSession.Store(product);
            await documentSession.SaveChangesAsync(cancellationToken);
            //Return CreateProductResult result
            return new CreateProductResult(product.Id);
        }
    }
}
