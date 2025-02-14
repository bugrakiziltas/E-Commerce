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
        cfg.Host(builder.Configuration["Rabbitmq:Host"],"/", host=>{
            host.Username(builder.Configuration.GetValue("Rabbitmq:Username","guest"));
            host.Username(builder.Configuration.GetValue("Rabbitmq:Password","guest"));
        });
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
using var scope=app.Services.CreateScope();
var services=scope.ServiceProvider;
try{
    var context=services.GetRequiredService<OrderDbContext>();
    await context.Database.MigrateAsync();
}catch (Exception ex){
     var logger=services.GetService<ILogger<Program>>();
     logger.LogError(ex, "An error occured during migration");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
