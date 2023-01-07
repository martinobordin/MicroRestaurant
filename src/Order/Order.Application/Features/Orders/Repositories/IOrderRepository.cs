using Order.Application.Contracts.Persistence;

namespace Order.Application.Features.Orders.Repositories;

public interface IOrderRepository : IRepository<Domain.Order>
{
    Task<IEnumerable<Domain.Order>> GetOrdersByUsernameAsync(string username);
}
