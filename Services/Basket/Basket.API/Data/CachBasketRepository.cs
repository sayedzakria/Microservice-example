

namespace Basket.API.Data
{
    public class CachBasketRepository (IBsaketRepository repository,IDistributedCache cache)
        : IBsaketRepository
    {

        public async Task<ShopingCart> GetBasket(string UserName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(UserName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShopingCart>(cachedBasket);
            var basket = await repository.GetBasket(UserName, cancellationToken);
            await cache.SetStringAsync(UserName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;

        }

        public async Task<ShopingCart> StoreBasket(ShopingCart basket, CancellationToken cancellationToken = default)
        {
             await repository.StoreBasket(basket, cancellationToken);
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);
            return true;
        }

    }
}
