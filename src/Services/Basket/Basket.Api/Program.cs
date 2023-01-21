using Basket.Api.Data;
using Basket.Api.Services;
using Common.Logging;
using Common.Logging.Extensions;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.Configure(options =>
      {
          options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId;
      });

builder.Host.UseSerilog(SerilogConfigurator.ConfigureSerilog);

// Add services to the container.
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["DiscountSettings:GrpcUrl"]!);
});

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        var host = builder.Configuration.GetConnectionString("RabbitMq");
        cfg.Host(host);
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddTelemetry(opt =>
{
    opt.ServiceName = "Basket.Api";
    opt.JaegerEndpoint = builder.Configuration["JaegerConfiguration:Endpoint"]!;
    opt.ZipkinEndpoint = builder.Configuration["ZipkinConfiguration:Endpoint"]!;
});

builder.Services.AddHealthChecks()
                    .AddRedis(builder.Configuration.GetConnectionString("Redis")!, "Redis Health", HealthStatus.Degraded);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
