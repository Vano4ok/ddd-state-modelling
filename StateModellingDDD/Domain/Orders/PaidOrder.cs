namespace StateModellingDDD.Domain.Orders;

public sealed partial class Order
{
    public sealed class Paid : Base
    {
        internal Paid(Order representation) : base(representation)
        {
        }
        
        public Result<Shipped>Ship()
        {
            ThrowIfInvalidState();
            
            Representation.Status = OrderStatus.Shipped;
            
            return new Shipped(Representation);
        }
        
        public Result<Canceled> Cancel()
        {
            ThrowIfInvalidState();
            
            Representation.Status = OrderStatus.Canceled;
            
            return new Canceled(Representation);
        }
        
        private void ThrowIfInvalidState()
        {
            if (Representation.Status is not OrderStatus.Paid)
            {
                throw new InvalidStatusException(OrderStatus.Paid);
            }
        }

        public override T Match<T>(
            Func<Pending, T> pendingFunc,
            Func<Paid, T> paidFunc,
            Func<Shipped, T> shippedFunc,
            Func<Completed, T> completedFunc,
            Func<Canceled, T> canceledFunc)
        {
            return paidFunc(this);
        }
    }
}
