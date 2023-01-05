using Basket.Api.Entities;

namespace Basket.Api.Data
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasketAsync(string username);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);
        Task DeleteBasketAsync(string username);
    }
}
