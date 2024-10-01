namespace StateModellingDDD.Domain.Orders;

public class OrderFactory
{
    public Order.Base ToModel(Order representation)
    {
        return representation.Status switch
        {
            OrderStatus.Pending => new Order.Pending(representation),
            OrderStatus.Paid => new Order.Paid(representation),
            OrderStatus.Shipped => new Order.Shipped(representation),
            OrderStatus.Completed => new Order.Completed(representation),
            OrderStatus.Canceled => new Order.Canceled(representation),
            _ => throw new ArgumentException("Unknown order state"),
        };
    }
}
