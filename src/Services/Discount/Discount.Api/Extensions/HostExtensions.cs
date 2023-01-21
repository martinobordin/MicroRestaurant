using Npgsql;
using Polly;

namespace Discount.Api.Extensions;

public static class HostExtensions
{
    public static IHost InitializeDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<IHost>>();

            try
            {
                logger.LogInformation("Migrating PostgreSQL database.");

                var retry = Policy.Handle<NpgsqlException>()
                       .WaitAndRetry(
                           retryCount: 5,
                           sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                           onRetry: (exception, retryCount, context) =>
                           {
                               logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                           });

                retry.Execute(() => ExecuteMigrations(configuration));



                logger.LogInformation("Migrated PostgreSQL database.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the PostgreSQL database");
            }
        }

        return host;
    }

    private static void ExecuteMigrations(IConfiguration configuration)
    {
        using var connection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSQL"));
        connection.Open();

        using var command = new NpgsqlCommand
        {
            Connection = connection
        };

        command.CommandText = @"SELECT EXISTS (
                    SELECT * FROM pg_tables
                    WHERE tablename  = 'coupon');";
        var couponTableExists = (bool?)command.ExecuteScalar();

        if (couponTableExists == true)
        {
            return;
        }

        command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Spicy lobster', 'Spicy lobster Weekend Discount', 5);";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Clam Zuppa', 'Clam Zuppa Special Discount', 10);";
        command.ExecuteNonQuery();
    }
}