using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
  private readonly ICatalogContext _context;

  public ProductRepository(ICatalogContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Product>> GetProductsAsync()
  {
    return await _context.Products
      .Find(p => true)
      .ToListAsync();
  }

  public async Task<Product> GetProductAsync(string id)
  {
    return await _context.Products
      .Find(p => p.Id == id)
      .FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
  {
    // MongoDB filter for the elements that match all the values
    var filter = Builders<Product>.Filter.Eq(product => product.Name, name);

    return await _context.Products
      .Find(filter)
      .ToListAsync();
  }

  public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
  {
    // MongoDB filter for the elements that match all the values
    var filter = Builders<Product>.Filter.Eq(product => product.Category, categoryName);

    return await _context.Products
      .Find(filter)
      .ToListAsync();
  }

  public async Task CreateProductAsync(Product product)
  {
    await _context.Products.InsertOneAsync(product);
  }

  public async Task<bool> UpdateProductAsync(Product product)
  {
    var updatedResult = await _context
      .Products
      .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

    return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
  }

  public async Task<bool> DeleteProductByIdAsync(string id)
  {
    var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

    var deleteResult = await _context.Products.DeleteOneAsync(filter);

    return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
  }
}