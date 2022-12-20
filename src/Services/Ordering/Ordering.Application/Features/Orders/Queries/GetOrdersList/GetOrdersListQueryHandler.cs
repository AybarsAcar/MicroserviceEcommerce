using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

/// <summary>
/// Performs the query operations for a GetOrdersListQuery that returns a List of OrderVm response response
/// </summary>
public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderVm>>
{
  private readonly IOrderRepository _repository;
  private readonly IMapper _mapper;

  public GetOrdersListQueryHandler(IOrderRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<List<OrderVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
  {
    var orders = await _repository.GetOrdersByUsernameAsync(request.Username);
    return _mapper.Map<List<OrderVm>>(orders);
  }
}