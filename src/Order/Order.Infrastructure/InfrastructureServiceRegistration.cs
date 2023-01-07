using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Contracts.Mailing;
using Order.Application.Contracts.Persistence;
using Order.Application.Features.Orders.Repositories;
using Order.Infrastructure.Features.Orders;
using Order.Infrastructure.Mailing;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("SqlServer");
            options.UseSqlServer(connectionString);
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}