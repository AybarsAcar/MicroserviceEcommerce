using MediatR;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

/// <summary>
/// No response is returned so we do not specify any type inside the angle brackets
/// </summary>
public class UpdateOrderCommand : IRequest
{
  public int Id { get; set; }
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