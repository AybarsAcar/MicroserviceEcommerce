using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

/// <summary>
/// HTTP Client Service class to interact with Basket.API Microservices
/// we will consume the methods integrating with the Basket Microservices
/// </summary>
public interface IBasketService
{
  Task<BasketModel> GetBasketByUsernameAsync(string username);
}