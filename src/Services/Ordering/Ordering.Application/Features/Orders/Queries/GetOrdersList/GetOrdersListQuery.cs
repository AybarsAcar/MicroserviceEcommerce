using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

/// <summary>
/// Query to get list of orders
/// IRequest will return a list of orderVm objects
/// </summary>
public class GetOrdersListQuery : IRequest<List<OrderVm>>
{
  public string Username { get; set; }

  public GetOrdersListQuery(string username)
  {
    Username = username;
  }
}