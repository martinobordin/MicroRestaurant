using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext catalogContext;

    public ProductRepository(ICatalogContext catalogContext)
    {
        this.catalogContext = catalogContext;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await this.catalogContext.Products.Find(p => true).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        return await this.catalogContext.Products.Find(p => p.Name.Contains(name)).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        return await this.catalogContext.Products.Find(p => p.Category == category).ToListAsync();
    }

    public async Task<Product> GetProductAsync(string id)
    {
        return await this.catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await this.catalogContext.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        var result = await this.catalogContext.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return product;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var result = await this.catalogContext.Products.DeleteOneAsync(p => p.Id == id);
        return result.IsAcknowledged && result.DeletedCount == 1;
    }
}
