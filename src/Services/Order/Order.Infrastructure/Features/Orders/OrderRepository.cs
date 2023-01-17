using Microsoft.EntityFrameworkCore;
using Order.Application.Features.Orders.Repositories;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure.Features.Orders;

public class OrderRepository : Repository<Domain.Order>, IOrderRepository
{
    public OrderRepository(OrderContext dbContext)
        : base(dbContext)
    {
    }
    public async Task<IEnumerable<Domain.Order>> GetOrdersByUsernameAsync(string username)
    {
        return await dbContext.Set<Domain.Order>().Where(o => o.UserName == username).ToListAsync();
    }
}
