namespace StateModellingDDD.Domain.Orders;

public class InvalidStatusException : Exception
{
    public InvalidStatusException(OrderStatus status)
        : base($"Status of order is invalid. It is not {status.ToString()}")
    {
    }
}
