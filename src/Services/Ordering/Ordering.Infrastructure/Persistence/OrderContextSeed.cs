using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
  public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
  {
    if (context.Orders.Any())
      return;

    context.Orders.AddRange(GetPreconfiguredOrders());

    await context.SaveChangesAsync();

    logger.LogInformation("Seed database associated with with context {DbContextName}", typeof(OrderContext));
  }

  private static IEnumerable<Order> GetPreconfiguredOrders()
  {
    return new List<Order>
    {
      new Order
      {
        Username = "default_user",
        FirstName = "Aybars",
        LastName = "Acar",
        EmailAddress = "aybars@gmail.com",
        AddressLine = "Hornsby",
        Country = "Australia",
        TotalPrice = 350
      }
    };
  }
}