using MediatR;

namespace SimpleMonolith.Api.Application.Orders.Queries.GetOrder
{
    public sealed record GetOrderQuery(int OrderId)
    : IRequest<OrderDto?>;
}
