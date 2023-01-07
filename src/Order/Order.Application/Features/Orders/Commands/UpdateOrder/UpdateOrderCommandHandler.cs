using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Exception;
using Order.Application.Features.Orders.Repositories;

namespace Order.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository orderRepository;
    private readonly IMapper mapper;
    private readonly ILogger<UpdateOrderCommandHandler> logger;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
    {
        this.orderRepository = orderRepository;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await orderRepository.GetByIdAsync(request.Id);
        if (orderToUpdate == null)
        {
            throw new EntityNotFoundException(nameof(Order), request.Id);
        }

        mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Domain.Order));

        await orderRepository.UpdateAsync(orderToUpdate);

        logger.LogInformation("Order {order} is successfully updated.", orderToUpdate);

        return Unit.Value;
    }
}
