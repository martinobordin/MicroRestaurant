using Common.Logging;
using Common.Logging.Extensions;
using Discount.Api.Data;
using Discount.Api.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SerilogConfigurator.ConfigureSerilog);

// Add services to the container.
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

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
    opt.ServiceName = "Discount.Api";
    opt.JaegerEndpoint = builder.Configuration["JaegerConfiguration:Endpoint"]!;
    opt.ZipkinEndpoint = builder.Configuration["ZipkinConfiguration:Endpoint"]!;
});
builder.Services.AddHealthChecks()
                    .AddNpgSql(builder.Configuration.GetConnectionString("PostgreSQL")!, failureStatus: HealthStatus.Degraded);

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

app.InitializeDatabase();

app.Run();
