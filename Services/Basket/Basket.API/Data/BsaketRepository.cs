


namespace Basket.API.Data
{
    public class BsaketRepository(IDocumentSession session) 
        : IBsaketRepository
    {
       
        public async Task<ShopingCart> GetBasket(string UserName, CancellationToken cancellationToken = default)
        {
            var basket=await session.LoadAsync<ShopingCart>(UserName, cancellationToken);
            return basket is null ? throw new BasketNotFoundException(UserName):basket;
        }

        public async Task<ShopingCart> StoreBasket(ShopingCart basket, CancellationToken cancellationToken = default)
        {
            session.Store(basket);
            await session.SaveChangesAsync(cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            session.Delete<ShopingCart>(userName);
            await session.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
