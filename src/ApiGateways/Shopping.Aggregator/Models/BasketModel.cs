namespace Shopping.Aggregator.Models;

/// <summary>
/// Shopping cart and Shopping Cart items
/// we will extend the Shopping cart item with the catalog model to include the description etc...
/// </summary>
public class BasketModel
{
  public string Username { get; set; }
  public List<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();
  public decimal TotalPrice { get; set; }
}