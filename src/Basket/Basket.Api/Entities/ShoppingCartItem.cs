﻿namespace Basket.Api.Entities;

public class ShoppingCartItem
{
    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string ProductId { get; set; } = default!;

    public string ProductName { get; set; } = default!;
}
