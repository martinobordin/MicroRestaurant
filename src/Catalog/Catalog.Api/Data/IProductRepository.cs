using Catalog.Api.Entities;

namespace Catalog.Api.Data;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);

    Task<Product> GetProductAsync(string id);

    Task<Product> CreateProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task<bool> DeleteProduct(string id);
}
