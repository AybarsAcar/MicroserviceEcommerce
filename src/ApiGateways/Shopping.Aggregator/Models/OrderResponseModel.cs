namespace Shopping.Aggregator.Models;

public class OrderResponseModel
{
  public string Username { get; set; }
  public decimal TotalPrice { get; set; }

  // BillingAddress - TODO: Extract into its own entity
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string EmailAddress { get; set; }
  public string AddressLine { get; set; }
  public string Country { get; set; }
  public string State { get; set; }
  public string ZipCode { get; set; }

  // Payment - TODO: Extract into its own entity
  public string CardName { get; set; }
  public string CardNumber { get; set; }
  public string Expiration { get; set; }
  public string Cvv { get; set; }
  public int PaymentMethod { get; set; } 
}