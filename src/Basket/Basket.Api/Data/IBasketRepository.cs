using Basket.Api.Entities;

namespace Basket.Api.Data
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasketAsync(string username, CancellationToken token = default);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken token = default);
        Task DeleteBasketAsync(string username, CancellationToken token = default);
    }
}
