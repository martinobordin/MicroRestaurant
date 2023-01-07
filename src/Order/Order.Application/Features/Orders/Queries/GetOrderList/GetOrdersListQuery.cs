using MediatR;

namespace Order.Application.Features.Orders.Queries.GetOrderList;

public record GetOrdersListQuery(string Username) : IRequest<List<GetOrdersListResponse>>;
