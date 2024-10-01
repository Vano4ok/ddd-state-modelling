using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StateModellingDDD.Domain.Orders;
using StateModellingDDD.Domain.Orders.OrderItems;
using StateModellingDDD.Domain.Orders.OrderItems.ValueObjects;
using StateModellingDDD.Domain.Orders.ValueObjects;
using StateModellingDDD.Domain.Products;
using StateModellingDDD.Domain.Users;

namespace StateModellingDDD.Data.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(a => a.Value, value => new OrderId(value));

        builder.Property(o => o.OwnerId)
            .HasConversion(a => a.Value, value => new UserId(value));

        builder.OwnsOne(o => o.PaymentDetails);

        builder.OwnsMany(o => o.OrderLines, ConfigureOrderItems);
    }

    private static void ConfigureOrderItems(OwnedNavigationBuilder<Order, OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .HasConversion(a => a.Value, value => new OrderLineId(value));

        builder.Property(oi => oi.ProductId)
            .HasConversion(a => a.Value, value => new ProductId(value));
    }
}
