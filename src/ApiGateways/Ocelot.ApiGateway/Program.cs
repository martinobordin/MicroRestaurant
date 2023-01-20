using Common.Logging;
using Common.Logging.Extensions;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SerilogConfigurator.ConfigureSerilog);

builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());

builder.Configuration.AddJsonFile("ocelot.json");
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true);

//builder.Logging
//    .AddConfiguration(builder.Configuration.GetSection("Logging"))
//    .AddConsole()
//    .AddDebug();

builder.Services.AddTelemetry(opt =>
{
    opt.ServiceName = "ApiGateway";
    opt.JaegerEndpoint = builder.Configuration["JaegerConfiguration:Endpoint"]!;
    opt.ZipkinEndpoint = builder.Configuration["ZipkinConfiguration:Endpoint"]!;
});

var app = builder.Build();

app.UseOcelot();



app.Run();
