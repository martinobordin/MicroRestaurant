namespace Basket.IntegrationEvents;

public record BasketCheckoutEvent : IntegrationEvent
{
    public string UserName { get; set; }
    public decimal TotalPrice { get; set; }

    public string BillingAddressFirstName { get; set; }
    public string BillingAddressLastName { get; set; }
    public string BillingAddressEmailAddress { get; set; }
    public string BillingAddressAddressLine { get; set; }
    public string BillingAddressCountry { get; set; }
    public string BillingAddressState { get; set; }
    public string BillingAddressZipCode { get; set; }
}