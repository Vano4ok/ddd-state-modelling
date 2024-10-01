namespace StateModellingDDD.Domain.Orders.OrderItems.ValueObjects;

public sealed record OrderLineId(Guid Value)
{
    public static OrderLineId New() => new(Guid.NewGuid());
}
