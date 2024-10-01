using StateModellingDDD.Domain.Orders.ValueObjects;

namespace StateModellingDDD.Domain.Orders;

public interface IOrderRepository
{
    public Task<Order.Base?> GetById(OrderId id, CancellationToken cancellationToken);
    
    public void Add(Order.Base order);
}