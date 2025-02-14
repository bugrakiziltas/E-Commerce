using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stripe;
using StripeService.Data;
using StripeService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Configuration.AddUserSecrets<Program>().AddEnvironmentVariables();
var stripeSecretKey = builder.Configuration["Stripe:SecretKey"] ?? Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
var stripeWHKey = builder.Configuration["Stripe:WHKey"] ?? Environment.GetEnvironmentVariable("STRIPE_WH_KEY");
StripeConfiguration.ApiKey = stripeSecretKey;
builder.Services.Configure<StripeSettings>(options =>
{
    options.SecretKey = stripeSecretKey;
    options.WHSecret = stripeWHKey;
});

builder.Services.AddDbContext<StripeDbContext>(options=>{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddMassTransit(x=>{
    x.AddEntityFrameworkOutbox<StripeDbContext>(o=>{
        o.UseBusOutbox();
        o.UsePostgres();
        o.QueryDelay=TimeSpan.FromSeconds(10);
    });
    x.UsingRabbitMq((context,cfg)=>{
        cfg.Host(builder.Configuration["Rabbitmq:Host"],"/", host=>{
            host.Username(builder.Configuration.GetValue("Rabbitmq:Username","guest"));
            host.Username(builder.Configuration.GetValue("Rabbitmq:Password","guest"));
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
using var scope=app.Services.CreateScope();
var services=scope.ServiceProvider;
try{
    var context=services.GetRequiredService<StripeDbContext>();
    await context.Database.MigrateAsync();
}catch (Exception ex){
     var logger=services.GetService<ILogger<Program>>();
     logger.LogError(ex, "An error occured during migration");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
