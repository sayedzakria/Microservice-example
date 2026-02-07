

namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public CustomerId CustomerId { get; private set; } = default!;
        public OrderName OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalPrice
        {
            get => _orderItems.Sum(item => item.Price * item.Quantity);
            private set { }
        }

        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment
            };

            order.AddDomainEvent(new OrderCreatedEvent(order));
            return order;
        }
        public void updateorder(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            AddDomainEvent(new OrderUpdatedEvent(this));
        }
            public void AddOrderItem(ProductId productId, int quantity, decimal price)
            {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price)); 
            var orderItem = new OrderItem(Id, productId, quantity, price);
                _orderItems.Add(orderItem);
                AddDomainEvent(new OrderItemAddedEvent(this, orderItem));
        }
        public void RemoveOrderItem(OrderItemId orderItemId)
        {
            var orderItem = _orderItems.FirstOrDefault(item => item.Id == orderItemId);
            if (orderItem != null)
            {
                _orderItems.Remove(orderItem);
                AddDomainEvent(new OrderItemRemovedEvent(this, orderItem));
            }
        }
    }
}
