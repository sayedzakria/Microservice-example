namespace Basket.API.Models
{
    public class ShopingCart
    {
        public string UserName { get; set; } = default!;
        public List<ShopingCartItem> Items { get; set; } = new();
        public decimal TotalPrice=>Items.Sum(item=>item.Price * item.Quantity);
        public ShopingCart(string userName)
        {
            UserName = userName;
        }
        // Empty constructor for mapping purposes
        public ShopingCart()
        {
        }
    }
}
