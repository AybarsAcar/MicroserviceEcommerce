using System.Net;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
  private readonly IBasketRepository _repository;
  private readonly DiscountGrpcService _discountGrpcService;
  private readonly IPublishEndpoint _publishEndpoint;
  private readonly IMapper _mapper;
  private readonly ILogger<BasketController> _logger;

  public BasketController(
    IBasketRepository repository,
    DiscountGrpcService discountGrpcService,
    IPublishEndpoint publishEndpoint,
    IMapper mapper,
    ILogger<BasketController> logger
  )
  {
    _repository = repository;
    _discountGrpcService = discountGrpcService;
    _publishEndpoint = publishEndpoint;
    _mapper = mapper;
    _logger = logger;
  }

  [HttpGet("{username}", Name = "GetBasketAsync")]
  [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
  public async Task<ActionResult<ShoppingCart>> GetBasketAsync(string username)
  {
    var basket = await _repository.GetBasketAsync(username);

    // when the user add their first item to their basket
    // we will create an empty basket with the user's username
    return Ok(basket ?? new ShoppingCart(username));
  }

  [HttpPost]
  [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
  public async Task<ActionResult<ShoppingCart>> UpdateBasketAsync([FromBody] ShoppingCart cart)
  {
    // consume gRPC service from Discount.Grpc; calculate the latest prices of products
    foreach (var item in cart.Items)
    {
      var coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName);

      // deduct the discount
      item.Price -= coupon.Amount;
    }

    return Ok(await _repository.CreateUpdateBasketAsync(cart));
  }

  [HttpDelete("{username}", Name = "DeleteBasketAsync")]
  [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> DeleteBasketAsync(string username)
  {
    await _repository.DeleteBasketAsync(username);
    return Ok();
  }

  [Route("[action]")]
  [HttpPost]
  [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.Accepted)]
  [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
  {
    // get existing basket with total price
    var basket = await _repository.GetBasketAsync(basketCheckout.Username);
    if (basket == null)
    {
      return BadRequest();
    }

    // create basketCheckout & set TotalPrice on basketCheckout eventMessage
    var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
    eventMessage.TotalPrice = basket.TotalPrice;

    // send checkout event to rabbitmq
    await _publishEndpoint.Publish<BasketCheckoutEvent>(eventMessage);

    // remove the basket
    await _repository.DeleteBasketAsync(basket.Username);

    return Accepted();
  }
}