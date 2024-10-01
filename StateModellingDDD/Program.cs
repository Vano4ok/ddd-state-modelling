using Microsoft.EntityFrameworkCore;
using StateModellingDDD.Api;
using StateModellingDDD.Data;
using StateModellingDDD.Domain;
using StateModellingDDD.Domain.Orders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<OrderFactory>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("TestDb");
});

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapOrderEndpointsEndpoints();

app.Run();
