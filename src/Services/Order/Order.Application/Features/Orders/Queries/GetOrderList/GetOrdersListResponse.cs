namespace Order.Application.Features.Orders.Queries.GetOrderList;

public record GetOrdersListResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }

    // BillingAddress
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string AddressLine { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
