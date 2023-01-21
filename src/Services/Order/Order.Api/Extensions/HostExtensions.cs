using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Order.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost InitializeDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<TContext>();
                var logger = services.GetRequiredService<ILogger<IHost>>();

                try
                {
                    logger.LogInformation("Initializing SqlServer database.");

                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(
                            retryCount: 5,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            onRetry: (exception, retryCount, context) =>
                            {
                                logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                            });

                    retry.Execute(context.Database.EnsureCreated);

                    logger.LogInformation("Initialized SqlServer database.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while initializing the SqlServer database");
                }
            }

            return host;
        }
    }
}
