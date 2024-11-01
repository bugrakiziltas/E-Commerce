using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Data;
using ShoppingCartService.Service.IRepositories;
using ShoppingCartService.Service.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ShoppingCartDbContext>(options=>{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddHttpClient<IShoppingCartRepository,ShoppingCartRepository>();
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
                authBuilder.RequireRole("Admin");
            });

 });
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using var scope=app.Services.CreateScope();
var services=scope.ServiceProvider;
try{
    var context=services.GetRequiredService<ShoppingCartDbContext>();
    await context.Database.MigrateAsync();
}catch (Exception ex){
     var logger=services.GetService<ILogger<Program>>();
     logger.LogError(ex, "An error occured during migration");
}
app.Run();
