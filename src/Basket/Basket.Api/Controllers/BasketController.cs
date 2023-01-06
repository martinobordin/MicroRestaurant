using Basket.Api.Data;
using Basket.Api.Entities;
using Basket.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository basketRepository;
    private readonly IDiscountGrpcService discountGrpcService;

    public BasketController(IBasketRepository basketRepository, IDiscountGrpcService discountGrpcService)
    {
        this.basketRepository = basketRepository;
        this.discountGrpcService = discountGrpcService;
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
        foreach (var shoppingCartItem in shoppingCart.Items)
        {
            var productDiscount = await this.discountGrpcService.GetDiscountAsync(shoppingCartItem.ProductName);
            shoppingCartItem.Price -= productDiscount.Amount;
        }

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
