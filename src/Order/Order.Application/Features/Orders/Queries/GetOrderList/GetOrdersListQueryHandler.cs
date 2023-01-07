using AutoMapper;
using MediatR;
using Order.Application.Features.Orders.Repositories;

namespace Order.Application.Features.Orders.Queries.GetOrderList;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<GetOrdersListResponse>>
{
    private readonly IOrderRepository orderRepository;
    private readonly IMapper mapper;

    public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        this.orderRepository = orderRepository;
        this.mapper = mapper;
    }

    public async Task<List<GetOrdersListResponse>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        var orderList = await orderRepository.GetOrdersByUsernameAsync(request.Username);
        return mapper.Map<List<GetOrdersListResponse>>(orderList);
    }
}
