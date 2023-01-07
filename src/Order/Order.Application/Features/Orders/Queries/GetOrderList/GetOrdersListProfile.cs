using AutoMapper;
using Order.Application.Features.Orders.Commands.CheckoutOrder;

namespace Order.Application.Features.Orders.Queries.GetOrderList;

public class GetOrdersListProfile : Profile
{
    public GetOrdersListProfile()
    {
        CreateMap<Domain.Order, GetOrdersListResponse>().ReverseMap();
    }
}
