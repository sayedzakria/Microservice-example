

using Ordering.Domain.Enums;

namespace Ordering.Application.Dtos;

public record OrderDto
(
    Guid Id,
    Guid CusromerId,
    string OrderName,
    AddressDto ShippingAddress,
      AddressDto BillingAddress,
        PaymentDto Payment,
        OrderStatus Status,
        List<OrderItemDto> OrderItems
    );
