using Basket.API.Entities;

namespace Basket.API.Repositories;

public interface IBasketRepository
{
  Task<ShoppingCart> GetBasketAsync(string username);

  /// <summary>
  /// As Redis as a key value pair with username as the key, we will have only update method
  /// where we will just insert the whole basket for the user
  /// </summary>
  /// <param name="cart"></param>
  /// <returns></returns>
  Task<ShoppingCart> CreateUpdateBasketAsync(ShoppingCart cart);

  Task DeleteBasketAsync(string username);
}