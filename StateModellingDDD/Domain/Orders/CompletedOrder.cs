namespace StateModellingDDD.Domain.Orders;

public sealed partial class Order
{
    public sealed class Completed : Base
    {
        internal Completed(Order representation) : base(representation)
        {
        }
        
        public override T Match<T>(
            Func<Pending, T> pendingFunc,
            Func<Paid, T> paidFunc,
            Func<Shipped, T> shippedFunc,
            Func<Completed, T> completedFunc,
            Func<Canceled, T> canceledFunc)
        {
            return completedFunc(this);
        }
    }
}
