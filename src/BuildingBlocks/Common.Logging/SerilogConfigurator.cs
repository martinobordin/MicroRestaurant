using Microsoft.Extensions.Hosting;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace Common.Logging;

public static class SerilogConfigurator
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureSerilog =>
       (context, configuration) =>
       {

           configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(context.Configuration.GetValue<string>("ElasticConfiguration:Endpoint")!))
                    {
                        IndexFormat = $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 2,
                        NumberOfReplicas = 1
                    })
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .Enrich.With<LogEnricher>()
                .ReadFrom.Configuration(context.Configuration);
       };
}
