using Microsoft.EntityFrameworkCore;
using StateModellingDDD.Domain.Orders;
using StateModellingDDD.Domain.Orders.ValueObjects;

namespace StateModellingDDD.Data;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly OrderFactory _orderFactory;

    public OrderRepository(
        AppDbContext context,
        OrderFactory orderFactory)
    {
        _context = context;
        _orderFactory = orderFactory;
        
    }
    
    public async Task<Order.Base?> GetById(OrderId id, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order is null)
        {
            return default;
        }
        
        return _orderFactory.ToModel(order);
    }

    public void Add(Order.Base order)
    {
        _context.Add(order.Representation);
    }
}