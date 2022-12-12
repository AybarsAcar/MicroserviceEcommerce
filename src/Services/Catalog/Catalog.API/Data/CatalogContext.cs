using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
  public IMongoCollection<Product> Products { get; }

  public CatalogContext(IConfiguration configuration)
  {
    // create the MongoDB client
    var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
    var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

    // populate the products collection
    Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

    // seed the data into the database
    CatalogContextSeed.SeedData(Products);
  }
}