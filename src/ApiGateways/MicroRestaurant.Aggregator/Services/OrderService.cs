using MicroRestaurant.Aggregator.Models;

namespace MicroRestaurant.Aggregator.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>?> GetOrdersByUserNameAsync(string userName);
    }
    public class OrderService : IOrderService
    {
        private readonly HttpClient client;

        public OrderService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<OrderResponseModel>?> GetOrdersByUserNameAsync(string userName)
        {
            var response = await client.GetFromJsonAsync<List<OrderResponseModel>>($"/api/v1/Order/{userName}");
            return response;
        }
    }
}
