using Basket.Api.Data;
using Basket.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository basketRepository;

    public BasketController(IBasketRepository basketRepository)
    {
        this.basketRepository = basketRepository;
    }

    [HttpGet("{username}")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
    {
        var basket = await this.basketRepository.GetBasketAsync(username);
        return Ok(basket ?? new ShoppingCart(username));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart shoppingCart)
    {
        var basket = await this.basketRepository.UpdateBasketAsync(shoppingCart);
        return Ok(basket);
    }

    [HttpDelete("{username}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteBasket(string username)
    {
        await this.basketRepository.DeleteBasketAsync(username);
        return Ok();
    }
}
