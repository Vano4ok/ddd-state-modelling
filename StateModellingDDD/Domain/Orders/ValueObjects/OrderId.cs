namespace StateModellingDDD.Domain.Orders.ValueObjects;

public sealed record OrderId(Guid Value)
{
    public static OrderId New() => new(Guid.NewGuid());
}
