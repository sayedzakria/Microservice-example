



namespace Ordering.Domain.Models
{
    public class OrderItem:Entity<Guid>
    {
        internal OrderItem(Guid orderId,Guid productId,int quanity,decimal price)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quanity;
            Price = price;
        }
        public Guid OrderId { get; private set; } = default!;
        public Guid ProductId { get; private set; } = default!;
        public int Quantity { get; private set; }= default!;
        public decimal Price { get; private set; }= default!;
    }
}
