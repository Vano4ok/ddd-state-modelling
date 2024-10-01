using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StateModellingDDD.Api.Requests;
using StateModellingDDD.Api.Responses;
using StateModellingDDD.Data;
using StateModellingDDD.Domain;
using StateModellingDDD.Domain.Orders;
using StateModellingDDD.Domain.Orders.ValueObjects;
using StateModellingDDD.Domain.Users;

namespace StateModellingDDD.Api;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpointsEndpoints(this IEndpointRouteBuilder builder)
    {
        var routeGroupBuilder = builder
            .MapGroup("orders");

        routeGroupBuilder.MapGet("{id:guid}", GetById);
        routeGroupBuilder.MapPost(string.Empty, Create);
        routeGroupBuilder.MapPut("{id:guid}/process-payment", ProcessPayment);
        routeGroupBuilder.MapPut("{id:guid}/ship", Ship);
        routeGroupBuilder.MapPut("{id:guid}/mark-as-delivered", MarkAsDelivered);
        routeGroupBuilder.MapPut("{id:guid}/cancel", Cancel);
        

        return builder;
    }

    private static async Task<Results<Ok<OrderResponse>, NotFound>> GetById(
        Guid id,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var response = await dbContext.Orders
            .Where(o => o.Id == new OrderId(id))
            .Select(o => new OrderResponse
            {
                Id = o.Id.Value,
                OwnerId = o.OwnerId.Value,
                CreatedAt = o.CreatedAt,
                PaymentMethod = o.PaymentDetails.PaymentMethod,
                Status = o.Status.ToString(),
                OrderItems = o.OrderLines.Select(ol => new OrderItem
                {
                    Id = ol.Id.Value,
                    ProductId = ol.ProductId.Value,
                    Quantity = ol.Quantity,
                }).ToArray()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(response);
    }
    
    private static async Task<Results<Ok<Guid>, NotFound>> Create(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var exampleOwnerId = new UserId(Guid.NewGuid());
        
        var order = Order.Create(
            exampleOwnerId,
            DateTime.Now);
        
        orderRepository.Add(order);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(order.Representation.Id.Value);
    }
    
    private static async Task<Results<NoContent, NotFound, BadRequest<Error>>> ProcessPayment(
        Guid id,
        ProcessPaymentRequest request,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetById(new OrderId(id), cancellationToken);

        if (order is null)
        {
            return TypedResults.NotFound();
        }

        var paymentDetails = new PaymentDetails(request.PaymentMethod);

        Result result = order.Match(
            pending => pending.ProcessPayment(paymentDetails),
            paid => new Error("Order is already paid"),
            shipped => new Error("Order is already shipped"),
            completed => new Error("Order is already completed"),
            canceled => new Error("Order is already canceled"));

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
    
    private static async Task<Results<NoContent, NotFound, BadRequest<Error>>> Ship(
        Guid id,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetById(new OrderId(id), cancellationToken);

        if (order is null)
        {
            return TypedResults.NotFound();
        }

        Result result = order.Match(
            pending => new Error("Order is not paid"),
            paid => paid.Ship(),
            shipped => new Error("Order is already shipped"),
            completed => new Error("Order is already completed"),
            canceled => new Error("Order is already canceled"));

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
    
    private static async Task<Results<NoContent, NotFound, BadRequest<Error>>> MarkAsDelivered(
        Guid id,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetById(new OrderId(id), cancellationToken);

        if (order is null)
        {
            return TypedResults.NotFound();
        }

        Result result = order.Match(
            pending => new Error("Order is not paid"),
            paid => new Error("Order is already shipped"),
            shipped => shipped.MarkAsDelivered(),
            completed => new Error("Order is already completed"),
            canceled => new Error("Order is already canceled"));

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
    
    private static async Task<Results<NoContent, NotFound, BadRequest<Error>>> Cancel(
        Guid id,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetById(new OrderId(id), cancellationToken);

        if (order is null)
        {
            return TypedResults.NotFound();
        }

        Result result = order.Match(
            pending => pending.Cancel(),
            paid => paid.Cancel(),
            shipped => shipped.Cancel(),
            completed => new Error("Order is already completed"),
            canceled => new Error("Order is already canceled"));

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
