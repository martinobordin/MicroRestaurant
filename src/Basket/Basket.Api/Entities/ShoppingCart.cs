namespace Basket.Api.Entities;

public class ShoppingCart
{

    /// <summary>
    /// Initializes a new instance of the <see cref="ShoppingCart"/> class.
    /// </summary>
    /// <param name="username">The username.</param>
    public ShoppingCart(string username)
    {
        Username = username;
    }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username.
    /// </value>
    public string Username { get; set; } = default!;

    /// <summary>
    /// Gets or sets the items.
    /// </summary>
    /// <value>
    /// The items.
    /// </value>
    public List<ShoppingCartItem> Items { get; set; } = new();

    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            foreach (var item in Items)
            {
                totalPrice += item.Price * item.Quantity;
            }
            return totalPrice;
        }
    }
}
