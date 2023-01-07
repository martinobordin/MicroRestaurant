using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Mailing;
using Order.Application.Features.Orders.Repositories;
using Order.Domain;

namespace Order.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository orderRepository;
    private readonly IEmailService emailService;
    private readonly IMapper mapper;
    private readonly ILogger<CheckoutOrderHandler> logger;

    public CheckoutOrderHandler(IOrderRepository orderRepository, IEmailService emailService, IMapper mapper, ILogger<CheckoutOrderHandler> logger)
    {
        this.orderRepository = orderRepository;
        this.emailService = emailService;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var order = mapper.Map<Domain.Order>(request);
        var newOrder = await orderRepository.AddAsync(order);

        logger.LogInformation("Order {OrderId} is successfully created.", newOrder.Id);

        await SendMail(newOrder);

        return newOrder.Id;
    }

    private async Task SendMail(Domain.Order order)
    {
        var email = new Email() { To = "test@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

        try
        {
            await emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "Order {OrderId} failed due to an error with the mail service", order.Id);
        }
    }
}