using Catalog.Api.Data;
using Catalog.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository productRepository;
    private readonly ILogger<CatalogController> logger;

    public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
    {
        this.productRepository = productRepository;
        this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]    
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = await this.productRepository.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{name}")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name)
    {
        var products = await this.productRepository.GetProductsByNameAsync(name);
        return Ok(products);
    }

    [HttpGet("{category}")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
    {
        var products = await this.productRepository.GetProductsByCategoryAsync(category);
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct(string id)
    {
        var product = await this.productRepository.GetProductAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await this.productRepository.CreateProduct(product);
        return CreatedAtRoute(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> UpdateProduct(Product product)
    {
        await this.productRepository.UpdateProduct(product);
        return Ok(product);
    }


    [HttpDelete]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> DeleteProduct(string id)
    {
        await this.productRepository.DeleteProduct(id);
        return Ok();
    }
}