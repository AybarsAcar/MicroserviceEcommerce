namespace Shopping.Aggregator.Models;

/// <summary>
/// Catalog related properties, same as the Product.cs model
/// </summary>
public class CatalogModel
{
  public string Id { get; set; }
  public string Name { get; set; }
  public string Category { get; set; }
  public string Summary { get; set; }
  public string Description { get; set; }
  public string ImageUrl { get; set; }
  public decimal Price { get; set; }
}