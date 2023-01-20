using Catalog.Api.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Catalog.Api.Data;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        //var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        //client.Settings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());

        var clientSettings = MongoClientSettings.FromConnectionString(configuration.GetConnectionString("MongoDb"));
        clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        var client = new MongoClient(clientSettings);
        var database = client.GetDatabase("CatalogDb");

        Products = database.GetCollection<Product>("Products");
        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}
