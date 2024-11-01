using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Data;
using ProductService.Services.IRepositories;
using ProductService.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ProductDbContext>(options=>{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x=>{
    x.AddEntityFrameworkOutbox<ProductDbContext>(o=>{
        o.UseBusOutbox();
        o.UsePostgres();
        o.QueryDelay=TimeSpan.FromSeconds(10);
    });
    x.UsingRabbitMq((context,cfg)=>{
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
builder.Services.AddAuthorization(options =>
  {

        options.AddPolicy("Admin",
            authBuilder =>
            {
                authBuilder.RequireClaim("scope","productService");
                authBuilder.RequireRole("Admin");
            });

 });
var app = builder.Build();

using var scope=app.Services.CreateScope();
var services=scope.ServiceProvider;
try{
    var context=services.GetRequiredService<ProductDbContext>();
    await context.Database.MigrateAsync();
    await SeedData.SeedProducts(context);
}catch (Exception ex){
     var logger=services.GetService<ILogger<Program>>();
     logger.LogError(ex, "An error occured during migration");
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
