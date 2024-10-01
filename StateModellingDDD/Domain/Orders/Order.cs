using StateModellingDDD.Domain.Orders.OrderItems;
using StateModellingDDD.Domain.Orders.ValueObjects;
using StateModellingDDD.Domain.Users;

namespace StateModellingDDD.Domain.Orders;

public sealed partial class Order
{
    private List<OrderItem> _orderLines = [];
    
    public OrderId Id { get; private init; }
    
    public UserId OwnerId { get; private init; }
    
    public DateTime CreatedAt { get; private set; }
    
    public PaymentDetails PaymentDetails { get; private set; }
    
    public OrderStatus Status { get; private set; }
    
    public IReadOnlyList<OrderItem> OrderLines => _orderLines.AsReadOnly();

    public static Pending Create(
        UserId owner,
        DateTime createdAt)
    {
        var order = new Order
        {
            Id = OrderId.New(),
            OwnerId = owner,
            CreatedAt = createdAt,
        };
        
        return new Pending(order);
    }

    public abstract class Base
    {
        private protected Base(Order representation)
        {
            Representation = representation;
        }

        public Order Representation { get; }

        public abstract T Match<T>(
            Func<Pending, T> pendingFunc,
            Func<Paid, T> paidFunc,
            Func<Shipped, T> shippedFunc,
            Func<Completed, T> completedFunc,
            Func<Canceled, T> canceledFunc);
    }
}
