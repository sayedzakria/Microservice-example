namespace Basket.API.Data
{
    public interface IBsaketRepository
    {
        Task<ShopingCart> GetBasket(string UserName, CancellationToken cancellationToken = default);
        Task<ShopingCart> StoreBasket(ShopingCart basket, CancellationToken cancellationToken = default);
        Task<bool> DeleteBasket(string userName,CancellationToken  cancellationToken = default);
    }
}
