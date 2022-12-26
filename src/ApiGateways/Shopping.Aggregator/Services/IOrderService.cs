using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

/// <summary>
/// HTTP Client Service class to interact with Order.API Microservices
/// we will consume the methods integrating with the Order Microservices
/// </summary>
public interface IOrderService
{
  Task<IEnumerable<OrderResponseModel>> GetOrdersByUsernameAsync(string username);
}