using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

 builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());

builder.Configuration.AddJsonFile("ocelot.json");
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true);

builder.Logging
    .AddConfiguration(builder.Configuration.GetSection("Logging"))
    .AddConsole()
    .AddDebug();

var app = builder.Build();

 app.UseOcelot();



app.Run();
