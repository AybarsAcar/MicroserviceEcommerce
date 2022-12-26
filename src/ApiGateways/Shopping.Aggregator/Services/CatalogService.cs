using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CatalogService : ICatalogService
{
  private readonly HttpClient _httpClient;

  public CatalogService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<IEnumerable<CatalogModel>> GetCatalogsAsync()
  {
    var response = await _httpClient.GetAsync("/api/v1/Catalog");

    return await response.ReadContentAsync<List<CatalogModel>>();
  }

  public async Task<IEnumerable<CatalogModel>> GetCatalogsByCategoryAsync(string category)
  {
    var response = await _httpClient.GetAsync($"/api/v1/Catalog/GetProductByCategory/{category}");

    return await response.ReadContentAsync<List<CatalogModel>>();
  }

  public async Task<CatalogModel> GetCatalogByIdAsync(string id)
  {
    var response = await _httpClient.GetAsync($"/api/v1/Catalog/{id}");

    return await response.ReadContentAsync<CatalogModel>();
  }
}