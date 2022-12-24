using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer;

/// <summary>
/// Consumer class to Consume RabbitMQ event bus messages
/// This checks-out the order
/// </summary>
public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
  private readonly IMapper _mapper;
  private readonly IMediator _mediator;
  private readonly ILogger<BasketCheckoutConsumer> _logger;

  public BasketCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
  {
    _mapper = mapper;
    _mediator = mediator;
    _logger = logger;
  }

  public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
  {
    var checkoutOrderCmd = _mapper.Map<CheckoutOrderCommand>(context.Message);

    var result = await _mediator.Send(checkoutOrderCmd);

    _logger.LogInformation("BasketCheckoutEvent consumed successfully for order with id {newOrderId}", result);
  }
}