using StateModellingDDD.Domain.Orders.OrderItems.ValueObjects;
using StateModellingDDD.Domain.Products;

namespace StateModellingDDD.Domain.Orders.OrderItems;

public sealed class OrderItem
{
    public OrderLineId Id { get; private init; }
    
    public ProductId ProductId { get; private set; }
    
    public long Quantity { get; private set; }

    internal static OrderItem Create(
        ProductId productId,
        long quantity)
    {
        var orderItem = new OrderItem
        {
            Id = OrderLineId.New(),
            ProductId = productId,
            Quantity = quantity,
        };

        return orderItem;
    }
}
