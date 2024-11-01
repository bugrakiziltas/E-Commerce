using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stripe;
using StripeService.Data;
using StripeService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Configuration.AddUserSecrets<Program>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
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
        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
