using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

/// <summary>
/// To manage data operations for catalog related entities
/// </summary>
public interface ICatalogContext
{
  /// <summary>
  /// Products Collection (Table) in MongoDb
  /// </summary>
  IMongoCollection<Product> Products { get; }
}