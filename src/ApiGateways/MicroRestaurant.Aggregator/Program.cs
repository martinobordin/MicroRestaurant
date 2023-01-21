using Common.Logging;
using Common.Logging.Extensions;
using HealthChecks.UI.Client;
using MicroRestaurant.Aggregator.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.Extensions.Http;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SerilogConfigurator.ConfigureSerilog);

// Add services to the container.
builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]!))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler((services, request) => GetRetryPolicy(services.GetRequiredService<ILogger<CatalogService>>()))
    .AddPolicyHandler((services, request) => GetCircuitBreakerPolicy(services.GetRequiredService<ILogger<CatalogService>>()));

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]!))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler((services, request) => GetRetryPolicy(services.GetRequiredService<ILogger<BasketService>>()))
    .AddPolicyHandler((services, request) => GetCircuitBreakerPolicy(services.GetRequiredService<ILogger<BasketService>>()));

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderUrl"]!))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler((services, request) => GetRetryPolicy(services.GetRequiredService<ILogger<OrderService>>()))
    .AddPolicyHandler((services, request) => GetCircuitBreakerPolicy(services.GetRequiredService<ILogger<OrderService>>()));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTelemetry(opt =>
{
    opt.ServiceName = "Aggregator";
    opt.JaegerEndpoint = builder.Configuration["JaegerConfiguration:Endpoint"]!;
    opt.ZipkinEndpoint = builder.Configuration["ZipkinConfiguration:Endpoint"]!;
});

builder.Services.AddHealthChecks()
              .AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:CatalogUrl"]!}/swagger/index.html"), "Catalog.API", HealthStatus.Degraded)
              .AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:BasketUrl"]!}/swagger/index.html"), "Basket.API", HealthStatus.Degraded)
              .AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:OrderUrl"]!}/swagger/index.html"), "Ordering.API", HealthStatus.Degraded);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();


static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy<T>(ILogger<T> logger)
{
    // In this case will wait for
    //  2 ^ 1 = 2 seconds then
    //  2 ^ 2 = 4 seconds then
    //  2 ^ 3 = 8 seconds then
    //  2 ^ 4 = 16 seconds then
    //  2 ^ 5 = 32 seconds

    return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (result, retryCount, context) =>
                {
                    logger.LogError("Retry {retryCount} of {context} due to: {exception}.", retryCount, context.PolicyKey, result.Exception);
                });

}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy<T>(ILogger<T> logger)
{
    return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (result, span) =>
                {
                    logger.LogError("Circuit is break due to: {exception}", result.Exception);
                },
                onReset: () =>
                {
                    logger.LogError("Circuit reset");
                },
                onHalfOpen: () =>
                {
                    logger.LogError("Circuit is Half open");
                }
            );
}

