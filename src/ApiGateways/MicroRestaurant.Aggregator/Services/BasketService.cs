using MicroRestaurant.Aggregator.Models;

namespace MicroRestaurant.Aggregator.Services
{
    public interface IBasketService
    {
        Task<BasketModel?> GetBasketAsync(string userName);
    }
    public class BasketService : IBasketService
    {
        private readonly HttpClient client;

        public BasketService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<BasketModel?> GetBasketAsync(string userName)
        {
            var response = await client.GetFromJsonAsync<BasketModel?>($"/api/v1/Basket/{userName}");
            return response;
        }
    }
}
