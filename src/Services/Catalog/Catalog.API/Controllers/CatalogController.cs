using System.Net;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
  private readonly IProductRepository _repository;
  private readonly ILogger<CatalogController> _logger;

  public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
  {
    _repository = repository;
    _logger = logger;
  }

  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<Product>), statusCode: (int)HttpStatusCode.OK)]
  public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
  {
    var products = await _repository.GetProductsAsync();
    return Ok(products);
  }

  [HttpGet("{id:length(24)}", Name = "GetProduct")]
  [ProducesResponseType((int)HttpStatusCode.NotFound)]
  [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
  public async Task<ActionResult<Product>> GetProductByIdAsync([FromQuery] string id)
  {
    var product = await _repository.GetProductAsync(id);

    if (product == null)
    {
      _logger.LogError($"Product with id: {id} is not found");
      return NotFound();
    }

    return Ok(product);
  }

  [Route("[action]/{category}", Name = "GetProductByCategory")]
  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<Product>), statusCode: (int)HttpStatusCode.OK)]
  public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategoryAsync([FromQuery] string category)
  {
    var products = await _repository.GetProductsByCategoryAsync(category);
    return Ok(products);
  }

  [HttpPost]
  [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
  public async Task<ActionResult<Product>> CreateProductAsync([FromBody] Product product)
  {
    await _repository.CreateProductAsync(product);

    return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
  }

  [HttpPut]
  [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> UpdateProductAsync([FromBody] Product product)
  {
    return Ok(await _repository.UpdateProductAsync(product));
  }

  [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
  [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> DeleteProductByIdAsync([FromQuery] string id)
  {
    return Ok(await _repository.DeleteProductByIdAsync(id));
  }
}