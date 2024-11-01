using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Consumers;
using SearchService.Data;
using SearchService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x=>{
    x.AddConsumersFromNamespaceContaining<ProductCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search",false));
    x.UsingRabbitMq((context,cfg)=>{
        cfg.ReceiveEndpoint("search-product-created",e=>{
            e.UseMessageRetry(r=>{
                r.Interval(5,5);
            });
            e.ConfigureConsumer<ProductCreatedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options=>{
    options.Authority=builder.Configuration["Url"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.NameClaimType="name";
    options.TokenValidationParameters.RoleClaimType=builder.Configuration["Role"];
});
builder.Services.AddHttpClient();
var app = builder.Build();

app.MapControllers();

try{
    await DbInit.InitDb(app);
}
catch(Exception ex){
    Console.WriteLine(ex);
}

app.Run();
