using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Data;

internal class CatalogContextSeed
{
    internal static void SeedData(IMongoCollection<Product> productCollection)
    {
        var productExists = productCollection.EstimatedDocumentCount() > 0;
        if (!productExists)
        {
            productCollection.InsertMany(GetSampleProducts());
        }
    }

    private static IEnumerable<Product> GetSampleProducts()
    {
        yield return new Product
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Spicy lobster",
            Price = 19,
            Category = "Appetizers"
        };

        yield return new Product
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Prosciutto Wrapped Mozzarella",
            Price = 14,
            Category = "Appetizers"
        };

        yield return new Product
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Filet",
            Price = 38,
            Category = "Steak cuts"
        };

        yield return new Product
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Ribs",
            Price = 32,
            Category = "Steak cuts"
        };

        yield return new Product
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Crab Cake",
            Price = 16,
            Category = "Seafoods"
        };

        yield return new Product
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Clam Zuppa",
            Price = 14.50M,
            Category = "Seafoods"
        };

        yield return new Product
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Caprese Salad",
            Price = 7.5M,
            Category = "Salads"
        };

        yield return new Product
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Spinach Salad",
            Price = 9.5M,
            Category = "Salads"
        };
    }
}