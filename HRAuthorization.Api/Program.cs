using Casbin;
using HRAuthorization.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
    });

var modelPath = Path.Combine(builder.Environment.ContentRootPath, "..", "CasbinModel", "rbac_with_domains.conf");
var policyPath = Path.Combine(builder.Environment.ContentRootPath, "..", "CasbinPolicy", "policy.csv");

builder.Services.AddSingleton<IEnforcer>(_ => new Enforcer(modelPath, policyPath));

builder.Services.AddAuthorization();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
