using Basket.IntegrationEvents;
using MassTransit;
using MediatR;
using Order.Application.Features.Orders.Commands.CheckoutOrder;

namespace Order.Application.Features.Orders.EventsConsumers;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator mediator;
    private readonly ILogger<BasketCheckoutConsumer> logger;

    public BasketCheckoutConsumer(IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var command = new CheckoutOrderCommand()
        {
            UserName = context.Message.UserName,
            TotalPrice = context.Message.TotalPrice,
            BillingAddressFirstName = context.Message.BillingAddressFirstName,
            BillingAddressLastName = context.Message.BillingAddressLastName,
            BillingAddressEmailAddress = context.Message.BillingAddressEmailAddress,
            BillingAddressAddressLine = context.Message.BillingAddressAddressLine,
            BillingAddressCountry = context.Message.BillingAddressCountry,
            BillingAddressState = context.Message.BillingAddressState,
            BillingAddressZipCode = context.Message.BillingAddressZipCode,
        };
        var result = await mediator.Send(command);

        logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);
    }
}
