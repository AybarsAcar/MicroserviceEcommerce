using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

/// <summary>
/// int represents the newly created order
/// </summary>
public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
  private readonly IOrderRepository _repository;
  private readonly IMapper _mapper;
  private readonly IEmailService _emailService;
  private readonly ILogger<CheckoutOrderCommandHandler> _logger;

  public CheckoutOrderCommandHandler(
    IOrderRepository repository,
    IMapper mapper,
    IEmailService emailService,
    ILogger<CheckoutOrderCommandHandler> logger
  )
  {
    _repository = repository;
    _mapper = mapper;
    _emailService = emailService;
    _logger = logger;
  }

  public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
  {
    var orderEntity = _mapper.Map<Order>(request);
    var createdOrder = await _repository.AddAsync(orderEntity);

    _logger.LogInformation($"Order {createdOrder.Id} is successfully created.");

    await SendEmail(createdOrder);

    return createdOrder.Id;
  }

  private async Task SendEmail(Order order)
  {
    var email = new Email
    {
      To = "aybars.dev@gmail.com",
      Subject = "New order is created",
      Body = $"Order {order.Id} is successfully created.",
    };

    try
    {
      await _emailService.SendEmailAsync(email);
    }
    catch (Exception e)
    {
      _logger.LogError($"Order {order.Id} failed due to an error with mail service {e.Message}");
    }
  }
}