using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options=>{
    options.Authority=builder.Configuration["Url"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.NameClaimType="name";
    options.TokenValidationParameters.RoleClaimType=builder.Configuration["Role"];
});
builder.Services.AddAuthorization(options =>{
    options.AddPolicy("Admin",
        authBuilder =>
        {
            authBuilder.RequireClaim("scope","productService");
            authBuilder.RequireRole("Admin");
        });
 });
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

app.Run();
