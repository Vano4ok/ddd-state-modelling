using StateModellingDDD.Domain.Orders.OrderItems;
using StateModellingDDD.Domain.Orders.ValueObjects;
using StateModellingDDD.Domain.Products;

namespace StateModellingDDD.Domain.Orders;

public sealed partial class Order
{
    public sealed class Pending : Base
    {
        internal Pending(Order representation) : base(representation)
        {
        }

        public Result AddProduct(
            ProductId productId,
            long quantity)
        {
            ThrowIfInvalidState();
            
            var orderItem = OrderItem.Create(
                productId,
                quantity);
            
            Representation._orderLines.Add(orderItem);
            
            return Result.Success();
        }

        public Result RemoveProduct(ProductId productId)
        {
            ThrowIfInvalidState();
            
            var orderItem = Representation._orderLines.SingleOrDefault(oi => oi.ProductId == productId);

            if (orderItem is null)
            {
                return Result.Success();
            }
            
            Representation._orderLines.Remove(orderItem);
            
            return Result.Success();
        }

        public Result<Paid> ProcessPayment(PaymentDetails paymentDetails)
        {
            ThrowIfInvalidState();
            
            Representation.PaymentDetails = paymentDetails;
            Representation.Status = OrderStatus.Paid;
            
            return new Paid(Representation);
        }
        
        public Result<Canceled> Cancel()
        {
            ThrowIfInvalidState();
            
            Representation.Status = OrderStatus.Canceled;

            return new Canceled(Representation);
        }
        
        private void ThrowIfInvalidState()
        {
            if (Representation.Status is not OrderStatus.Pending)
            {
                throw new InvalidStatusException(OrderStatus.Pending);
            }
        }
        
        public override T Match<T>(
            Func<Pending, T> pendingFunc,
            Func<Paid, T> paidFunc,
            Func<Shipped, T> shippedFunc,
            Func<Completed, T> completedFunc,
            Func<Canceled, T> canceledFunc)
        {
            return pendingFunc(this);
        }
    }
}
