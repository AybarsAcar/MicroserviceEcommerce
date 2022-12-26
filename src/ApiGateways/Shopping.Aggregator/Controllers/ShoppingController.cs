using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingController : ControllerBase
{
  private readonly ICatalogService _catalogService;
  private readonly IBasketService _basketService;
  private readonly IOrderService _orderService;

  public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
  {
    _catalogService = catalogService;
    _basketService = basketService;
    _orderService = orderService;
  }

  [HttpGet("{username}", Name = "GetShopping")]
  [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
  public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
  {
    // get basket with username
    var basket = await _basketService.GetBasketByUsernameAsync(username);

    // iterate basket items and consume products with basket item productId
    foreach (var item in basket.Items)
    {
      var product = await _catalogService.GetCatalogByIdAsync(item.ProductId);

      // set additional product fields onto basket item
      // map product related members into basket item dto with extended column
      item.ProductName = product.Name;
      item.Category = product.Category;
      item.Summary = product.Summary;
      item.Description = product.Description;
      item.ImageUrl = product.ImageUrl;
    }

    // consume ordering microservices in order to retrieve order list
    var orders = await _orderService.GetOrdersByUsernameAsync(username);

    // return root ShoppingModel DTO Class which includes all responses
    var shoppingModel = new ShoppingModel
    {
      Username = username,
      BasketWithProducts = basket,
      Orders = orders
    };

    return Ok(shoppingModel);
  }
}