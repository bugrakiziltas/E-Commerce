using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Consumers;
using OrderService.Data;
using OrderService.Services.IRepositories;
using OrderService.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddDbContext<OrderDbContext>(options=>{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddMassTransit(x=>{
    x.AddConsumersFromNamespaceContaining<OrderCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("order",false));
    x.UsingRabbitMq((context,cfg)=>{
        cfg.ReceiveEndpoint("order-order-created",e=>{
            e.UseMessageRetry(r=>{
                r.Interval(5,5);
            });
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
