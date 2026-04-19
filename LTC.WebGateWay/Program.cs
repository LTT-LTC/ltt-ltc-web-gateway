using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using LTC.Shared.ServiceDefaults;
using LTC.WebGateWay.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Global Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("PerIpRateLimit", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromSeconds(5),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });
});

// Add services to the container.
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var configuration = builder.Configuration.GetSection("ReverseProxy");
builder.Services.AddReverseProxy().LoadFromConfig(configuration);

var app = builder.Build();
var env = app.Environment;
app.MapDefaultEndpoints();

if (env.IsProduction() || env.IsDevelopment() || env.EnvironmentName == "LocalDevelopment" || env.EnvironmentName == "InternalDevelopment")
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "LTC Swagger";
        options.RoutePrefix = "ltc/swagger";
        var reverseProxyConfig = app.Configuration.GetSection("ReverseProxy");
        options.ConfigureSwaggerEndpoints(reverseProxyConfig);
    });
}

app.UseRateLimiter();

app.MapReverseProxy();
app.Run();