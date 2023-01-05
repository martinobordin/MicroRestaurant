using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task<ShoppingCart?> GetBasketAsync(string username)
        {
            var basket = await this.distributedCache.GetStringAsync(username);
            if (string.IsNullOrWhiteSpace(basket))
            {
                return null;
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            await this.distributedCache.SetStringAsync(basket.Username, JsonSerializer.Serialize(basket));
            return basket;
        }

        public async Task DeleteBasketAsync(string username)
        {
            await this.distributedCache.RemoveAsync(username);
        }
    }
}
