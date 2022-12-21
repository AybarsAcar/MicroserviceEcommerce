using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContext : DbContext
{
  public OrderContext(DbContextOptions<OrderContext> options) : base(options)
  {
  }

  // Tables
  public DbSet<Order> Orders { get; set; }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    // before saving entities preset some of the relation attributes
    foreach (var entry in ChangeTracker.Entries<EntityBase>())
    {
      switch (entry.State)
      {
        case EntityState.Added:
          entry.Entity.CreatedDate = DateTime.Now;
          entry.Entity.CreatedBy = "default_user";
          break;
        case EntityState.Modified:
          entry.Entity.LastModifiedDate = DateTime.Now;
          entry.Entity.LastModifiedBy = "default_user";
          break;
      }
    }

    return base.SaveChangesAsync(cancellationToken);
  }
}