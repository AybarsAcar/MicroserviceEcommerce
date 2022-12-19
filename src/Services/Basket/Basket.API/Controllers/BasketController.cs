using System.Net;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
  private readonly IBasketRepository _repository;
  private readonly DiscountGrpcService _discountGrpcService;
  private readonly ILogger<BasketController> _logger;

  public BasketController(
    IBasketRepository repository,
    DiscountGrpcService discountGrpcService,
    ILogger<BasketController> logger
  )
  {
    _repository = repository;
    _discountGrpcService = discountGrpcService;
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
}