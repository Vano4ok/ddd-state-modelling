using Microsoft.EntityFrameworkCore;
using StateModellingDDD.Domain;
using StateModellingDDD.Domain.Orders;

namespace StateModellingDDD.Data;

public class AppDbContext : DbContext, IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}