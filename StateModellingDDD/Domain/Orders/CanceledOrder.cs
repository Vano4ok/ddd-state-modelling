namespace StateModellingDDD.Domain.Orders;

public sealed partial class Order
{
    public sealed class Canceled : Base
    {
        internal Canceled(Order representation) : base(representation)
        {
        }
        
        public override T Match<T>(
            Func<Pending, T> pendingFunc,
            Func<Paid, T> paidFunc,
            Func<Shipped, T> shippedFunc,
            Func<Completed, T> completedFunc,
            Func<Canceled, T> canceledFunc)
        {
            return canceledFunc(this);
        }
    }
}