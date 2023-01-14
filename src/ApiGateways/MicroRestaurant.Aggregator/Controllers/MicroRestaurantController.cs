using MicroRestaurant.Aggregator.Models;
using MicroRestaurant.Aggregator.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MicroRestaurant.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MicroRestaurantController : ControllerBase
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;
        private readonly IOrderService orderService;

        public MicroRestaurantController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
            this.orderService = orderService;
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(MicroRestaurantModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MicroRestaurantModel>> GetShopping(string userName)
        {
            // get basket with username
            // iterate basket items and consume products with basket item productId member
            // map product related members into basketitem dto with extended columns
            // consume ordering microservices in order to retrieve order list
            // return root ShoppngModel dto class which including all responses

            var basketTask = basketService.GetBasketAsync(userName);
            var ordersTask = orderService.GetOrdersByUserNameAsync(userName);

            Task.WaitAll(basketTask, ordersTask);

            if (basketTask.Result != null)
            {
                foreach (var item in basketTask.Result.Items)
                {
                    var product = await catalogService.GetCatalogAsync(item.ProductId);

                    if (product != null)
                    {
                        // set additional product fields onto basket item
                        item.ProductName = product.Name;
                        item.Category = product.Category;
                        item.Summary = product.Summary;
                        item.Description = product.Description;
                        item.ImageFile = product.ImageFile;
                    }
                }
            }

            

            var shoppingModel = new MicroRestaurantModel
            {
                UserName = userName,
                BasketWithProducts = basketTask.Result,
                Orders = ordersTask.Result
            };

            return Ok(shoppingModel);
        }
    }
}