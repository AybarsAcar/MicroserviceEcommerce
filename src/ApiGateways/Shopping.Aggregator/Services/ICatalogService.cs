using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

/// <summary>
/// HTTP Client Service class to interact with Catalog.API Microservices
/// we will consume the methods integrating with the Catalog Microservices
/// </summary>
public interface ICatalogService
{
  Task<IEnumerable<CatalogModel>> GetCatalogsAsync();
  Task<IEnumerable<CatalogModel>> GetCatalogsByCategoryAsync(string category);
  Task<CatalogModel> GetCatalogByIdAsync(string id);
}