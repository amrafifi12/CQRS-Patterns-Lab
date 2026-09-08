using MediatR;

namespace SimpleMonolith.Api.Application.Orders.Notifications
{
    public sealed record OrderCreatedNotification(
        int OrderId,
        int CustomerId,
        decimal TotalAmount
    ) : INotification;
}
