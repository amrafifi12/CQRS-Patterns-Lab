using MediatR;
using SimpleMonolith.Api.Application.Abstractions.Persistence;
using SimpleMonolith.Api.Application.Orders.Notifications;

namespace SimpleMonolith.Api.Application.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPublisher _publisher;

        public CreateOrderCommandHandler(IApplicationDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Domain.Entities.Order(
            request.CustomerId,
            request.TotalAmount);

            _context.Orders.Add(order);

            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new OrderCreatedNotification(order.Id, order.CustomerId, request.TotalAmount));

            return order.Id;

        }
    }
}
