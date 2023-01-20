using Basket.Api.Data;
using Basket.Api.Entities;
using Basket.Api.Services;
using Basket.IntegrationEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository basketRepository;
    private readonly IDiscountGrpcService discountGrpcService;
    private readonly IPublishEndpoint publishEndpoint;

    public BasketController(IBasketRepository basketRepository, IDiscountGrpcService discountGrpcService, IPublishEndpoint publishEndpoint)
    {
        this.basketRepository = basketRepository;
        this.discountGrpcService = discountGrpcService;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpGet("{username}")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string username, CancellationToken token = default)
    {
        var basket = await this.basketRepository.GetBasketAsync(username, token);
        return Ok(basket ?? new ShoppingCart(username));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart shoppingCart, CancellationToken token = default)
    {
        foreach (var shoppingCartItem in shoppingCart.Items)
        {
            var productDiscount = await this.discountGrpcService.GetDiscountAsync(shoppingCartItem.ProductName, token);
            shoppingCartItem.Price -= productDiscount.Amount;
        }

        var basket = await this.basketRepository.UpdateBasketAsync(shoppingCart, token);
        return Ok(basket);
    }

    [HttpDelete("{username}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteBasket(string username, CancellationToken token = default)
    {
        await this.basketRepository.DeleteBasketAsync(username, token);
        return Ok();
    }

    [HttpPost("Checkout")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout , CancellationToken token = default)
    {
        var basket = await this.basketRepository.GetBasketAsync(basketCheckout.Username, token);
        if (basket == null)
        {
            return NotFound();
        }

        var eventMessage = new BasketCheckoutEvent()
        {
            UserName = basket.Username,
            TotalPrice = basket.TotalPrice,
            BillingAddressFirstName = basketCheckout.FirstName,
            BillingAddressLastName = basketCheckout.LastName,
            BillingAddressEmailAddress = basketCheckout.EmailAddress,
            BillingAddressAddressLine = basketCheckout.AddressLine,
            BillingAddressCountry = basketCheckout.Country,
            BillingAddressState = basketCheckout.State,
            BillingAddressZipCode = basketCheckout.ZipCode
        };

        //TODO: Use outbox pattern
        await this.publishEndpoint.Publish(eventMessage, token);
        await basketRepository.DeleteBasketAsync(basket.Username, token);

        return Accepted();
    }
}
