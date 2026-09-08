using MediatR;

namespace SimpleMonolith.Api.Application.Orders.Notifications.OrderCreated
{
    public sealed class OrderCreatedCustomerHandler
    : INotificationHandler<OrderCreatedNotification>
    {
        public Task Handle(
            OrderCreatedNotification notification,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(
                $"Notify customer {notification.CustomerId} " +
                $"about order {notification.OrderId}.");

            return Task.CompletedTask;
        }
    }
}
