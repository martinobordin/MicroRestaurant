using MediatR;

namespace Order.Application.Features.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }
}
