using StateModellingDDD.Domain.Orders;

namespace StateModellingDDD.Api.Responses;

public sealed record OrderResponse
{
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string? PaymentMethod { get; set; }
    
    public string Status { get; set; }
    
    public OrderItem[] OrderItems { get; set; }
}

public sealed record OrderItem
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    
    public long Quantity { get; set; }
}
