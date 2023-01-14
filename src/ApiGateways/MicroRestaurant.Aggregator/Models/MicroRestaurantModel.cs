namespace MicroRestaurant.Aggregator.Models
{
    public class MicroRestaurantModel
    {
        public string UserName { get; set; } = string.Empty;
        public BasketModel? BasketWithProducts { get; set; }
        public IEnumerable<OrderResponseModel>? Orders { get; set; }
    }
}
