
using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories;

public interface IDiscountRepository
{
  /// <summary>
  /// Returns the whole coupon given its product name
  /// </summary>
  /// <param name="productName"></param>
  /// <returns></returns>
  Task<Coupon> GetDiscountAsync(string productName);

  Task<bool> CreateDiscountAsync(Coupon coupon);

  Task<bool> UpdateDiscountAsync(Coupon coupon);

  Task<bool> DeleteDiscountAsync(string productName);
}