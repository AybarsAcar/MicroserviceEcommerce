namespace EventBus.Messages.Events;

/// <summary>
/// Event produced from Basket Microservices when a basket is checked-out
/// Consumed from Order Microservices
/// </summary>
public class BasketCheckoutEvent : IntegrationBaseEvent
{
  public string Username { get; set; }
  public decimal TotalPrice { get; set; }

  // BillingAddress
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string EmailAddress { get; set; }
  public string AddressLine { get; set; }
  public string Country { get; set; }
  public string State { get; set; }
  public string ZipCode { get; set; }

  // Payment
  public string CardName { get; set; }
  public string CardNumber { get; set; }
  public string Expiration { get; set; }
  public string Cvv { get; set; }
  public int PaymentMethod { get; set; }
}