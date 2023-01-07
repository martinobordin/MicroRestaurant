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

        public async Task<ShoppingCart?> GetBasketAsync(string username, CancellationToken token = default)
        {
            var basket = await this.distributedCache.GetStringAsync(username, token);
            if (string.IsNullOrWhiteSpace(basket))
            {
                return null;
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken token = default)
        {
            await this.distributedCache.SetStringAsync(basket.Username, JsonSerializer.Serialize(basket), token);
            return basket;
        }

        public async Task DeleteBasketAsync(string username, CancellationToken token = default)
        {
            await this.distributedCache.RemoveAsync(username, token);
        }
    }
}
