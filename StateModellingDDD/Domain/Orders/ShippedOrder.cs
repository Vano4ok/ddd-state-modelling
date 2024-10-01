namespace StateModellingDDD.Domain.Orders;

public sealed partial class Order
{
    public sealed class Shipped : Base
    {
        internal Shipped(Order representation) : base(representation)
        {
        }
        
        public Result<Completed> MarkAsDelivered()
        {
            ThrowIfInvalidState();
            
            Representation.Status = OrderStatus.Completed;

            return new Completed(Representation);
        }

        public Result<Canceled> Cancel()
        {
            ThrowIfInvalidState();
            
            Representation.Status = OrderStatus.Canceled;

            return new Canceled(Representation);
        }
        
        private void ThrowIfInvalidState()
        {
            if (Representation.Status is not OrderStatus.Shipped)
            {
                throw new InvalidStatusException(OrderStatus.Shipped);
            }
        }
        
        public override T Match<T>(
            Func<Pending, T> pendingFunc,
            Func<Paid, T> paidFunc,
            Func<Shipped, T> shippedFunc,
            Func<Completed, T> completedFunc,
            Func<Canceled, T> canceledFunc)
        {
            return shippedFunc(this);
        }
    }
}
