using MediatR;

namespace SimpleMonolith.Api.Application.Order.Commands.CreateOrder
{
    public sealed record CreateOrderCommand(
     int CustomerId,
     decimal TotalAmount) : IRequest<int>;
}
