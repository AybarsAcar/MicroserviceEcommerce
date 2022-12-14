using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
  private readonly IMediator _mediator;

  public OrderController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet("{username}", Name = "GetOrders")]
  [ProducesResponseType(typeof(IEnumerable<OrderVm>), (int)HttpStatusCode.OK)]
  public async Task<ActionResult<OrderVm>> GetOrdersByUsername(string username)
  {
    var query = new GetOrdersListQuery(username);
    var orders = await _mediator.Send(query);

    return Ok(orders);
  }

  /// <summary>
  /// TODO: Remove this as this method is only for testing
  /// this logic is handler in the BasketCheckoutConsumer
  /// </summary>
  /// <param name="command"></param>
  /// <returns></returns>
  [HttpPost(Name = "CheckoutOrder")]
  [ProducesResponseType((int)HttpStatusCode.OK)]
  public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
  {
    return Ok(await _mediator.Send(command));
  }

  [HttpPut(Name = "UpdateOrder")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
  {
    await _mediator.Send(command);
    return NoContent();
  }

  [HttpDelete("{id}", Name = "DeleteOrder")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> DeleteOrder(int id)
  {
    var command = new DeleteOrderCommand { OrderId = id };
    await _mediator.Send(command);
    return NoContent();
  }
}