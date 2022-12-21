using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
  public OrderRepository(OrderContext dbContext) : base(dbContext)
  {
  }

  public async Task<IEnumerable<Order>> GetOrdersByUsernameAsync(string username)
  {
    return await _dbContext.Orders
      .Where(order => order.Username == username)
      .ToListAsync();
  }
}