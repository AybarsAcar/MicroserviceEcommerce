using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence;

/// <summary>
/// Repository to interact with the Order table
/// based on IAsyncRepository with extra queries
/// </summary>
public interface IOrderRepository : IAsyncRepository<Order>
{
  /// <summary>
  /// Retrieves the order entities for a given username
  /// </summary>
  /// <param name="username"></param>
  /// <returns></returns>
  Task<IEnumerable<Order>> GetOrdersByUsernameAsync(string username);
}