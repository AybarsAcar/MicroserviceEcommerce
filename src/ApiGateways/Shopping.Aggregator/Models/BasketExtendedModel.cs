namespace Shopping.Aggregator.Models;

/// <summary>
/// This will extend the ShoppingCartItem to include the Catalog related properties
/// like description, imageUrl, etc...
/// </summary>
public class BasketItemExtendedModel
{
  public int Quantity { get; set; }
  public string Colour { get; set; }
  public decimal Price { get; set; }
  public string ProductId { get; set; }
  public string ProductName { get; set; }

  // Product Related Additional Fields from Catalog.API
  public string Category { get; set; }
  public string Summary { get; set; }
  public string Description { get; set; }
  public string ImageUrl { get; set; }
}