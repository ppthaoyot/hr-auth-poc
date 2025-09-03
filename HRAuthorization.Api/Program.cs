using Casbin;
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

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/authz/check", async (HttpContext ctx, IEnforcer enforcer, string dom, string obj, string act) =>
{
    var sub = ctx.User.Identity?.Name ?? string.Empty;
    var allowed = await enforcer.EnforceAsync(sub, dom, obj, act);
    return allowed ? Results.NoContent() : Results.StatusCode(StatusCodes.Status403Forbidden);
}).RequireAuthorization();

app.MapGet("/api/leave/{id}", async (HttpContext ctx, IEnforcer enforcer, string dom, string id) =>
{
    var sub = ctx.User.Identity?.Name ?? string.Empty;
    var allowed = await enforcer.EnforceAsync(sub, dom, "api.leave", "read");
    return allowed ? Results.Ok(new { Id = id }) : Results.StatusCode(StatusCodes.Status403Forbidden);
}).RequireAuthorization();

app.MapPost("/api/leave/{id}/approve", async (HttpContext ctx, IEnforcer enforcer, string dom, string id) =>
{
    var sub = ctx.User.Identity?.Name ?? string.Empty;
    var allowed = await enforcer.EnforceAsync(sub, dom, "api.leave", "approve");
    return allowed ? Results.NoContent() : Results.StatusCode(StatusCodes.Status403Forbidden);
}).RequireAuthorization();

app.Run();
