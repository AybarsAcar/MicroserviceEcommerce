namespace Shopping.Aggregator.Models;

/// <summary>
/// Combine and dispatch all the other model classes
/// </summary>
public class ShoppingModel
{
  public string Username { get; set; }
  public BasketModel BasketWithProducts { get; set; }
  public IEnumerable<OrderResponseModel> Orders { get; set; }
}