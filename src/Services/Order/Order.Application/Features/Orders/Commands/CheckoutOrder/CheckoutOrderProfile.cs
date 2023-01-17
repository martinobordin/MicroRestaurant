using AutoMapper;

namespace Order.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderProfile : Profile
{
    public CheckoutOrderProfile()
    {
        CreateMap<Domain.Order, CheckoutOrderCommand>().ReverseMap();
    }
}
