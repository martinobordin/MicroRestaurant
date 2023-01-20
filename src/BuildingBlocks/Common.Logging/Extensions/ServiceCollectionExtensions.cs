using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

namespace Common.Logging.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelemetry(this IServiceCollection serviceCollection, Action<TelemetryOptions> configureTelemetryOptions)
        {
            var telemetryOptions = new TelemetryOptions();
            configureTelemetryOptions.Invoke(telemetryOptions);

            serviceCollection.AddOpenTelemetryTracing((traceProviderBuilder) =>
            {
                traceProviderBuilder
                     .SetResourceBuilder((ResourceBuilder?)ResourceBuilder.CreateDefault().AddService(telemetryOptions.ServiceName))
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddMongoDBInstrumentation()
                    
                    //.AddSource(nameof(BasketController))
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = telemetryOptions.JaegerEndpoint.Split(":")[0];
                        options.AgentPort = int.Parse(telemetryOptions.JaegerEndpoint.Split(":")[1]);
                        options.ExportProcessorType = ExportProcessorType.Simple;
                    })
                    .AddZipkinExporter(zipkinOptions =>
                    {
                        zipkinOptions.Endpoint = new Uri(telemetryOptions.ZipkinEndpoint);
                    })
                    .AddConsoleExporter(options =>
                    {
                        options.Targets = ConsoleExporterOutputTargets.Console;
                    });
            });

            return serviceCollection;
        }
    }
}
