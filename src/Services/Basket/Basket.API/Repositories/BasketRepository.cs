using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
  private readonly IDistributedCache _redis;

  public BasketRepository(IDistributedCache redis)
  {
    _redis = redis;
  }

  public async Task<ShoppingCart> GetBasketAsync(string username)
  {
    var basketJson = await _redis.GetStringAsync(username);
    return string.IsNullOrEmpty(basketJson) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basketJson);
  }

  public async Task<ShoppingCart> CreateUpdateBasketAsync(ShoppingCart cart)
  {
    await _redis.SetStringAsync(cart.Username, JsonConvert.SerializeObject(cart));
    return await GetBasketAsync(cart.Username);
  }

  public async Task DeleteBasketAsync(string username)
  {
    await _redis.RemoveAsync(username);
  }
}