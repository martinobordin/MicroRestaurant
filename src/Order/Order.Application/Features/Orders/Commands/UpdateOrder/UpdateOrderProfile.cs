using AutoMapper;

namespace Order.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderProfile : Profile
{
    public UpdateOrderProfile()
    {
        CreateMap<Domain.Order, UpdateOrderCommand>().ReverseMap();
    }
}
