using MediatR;

namespace SimpleMonolith.Api.Application.Orders.Notifications.OrderCreated
{
    public sealed class OrderCreatedAuditHandler : INotificationHandler<OrderCreatedNotification>
    {
        public Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(
                       $"Order {notification.OrderId} was created.");

            return Task.CompletedTask;
        }
    }
}
