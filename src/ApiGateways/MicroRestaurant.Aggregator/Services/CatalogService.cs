using MicroRestaurant.Aggregator.Models;

namespace MicroRestaurant.Aggregator.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>?> GetCatalogAsync();
        Task<IEnumerable<CatalogModel>?> GetCatalogByCategoryAsync(string category);
        Task<CatalogModel?> GetCatalogAsync(string id);
    }
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient client;

        public CatalogService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<CatalogModel>?> GetCatalogAsync()
        {
            var response = await client.GetFromJsonAsync<List<CatalogModel>>("/api/v1/Catalog");
            return response;
        }

        public async Task<CatalogModel?> GetCatalogAsync(string id)
        {
            var response = await client.GetFromJsonAsync<CatalogModel>($"/api/v1/Catalog/{id}");
            return response;
        }

        public async Task<IEnumerable<CatalogModel>?> GetCatalogByCategoryAsync(string category)
        {
            var response = await client.GetFromJsonAsync<List<CatalogModel>>($"/api/v1/Catalog/GetProductByCategory/{category}");
            return response;
        }
    }
}
