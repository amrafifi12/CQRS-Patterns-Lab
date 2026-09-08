using MediatR;
using SimpleSeparateDb.Api.Application.Abstractions.Persistence;
using SimpleSeparateDb.Api.Application.Events;
using SimpleSeparateDb.Api.WriteInfrastructure.Persistence;
using System.Text.Json;

namespace SimpleSeparateDb.Api.Application.Orders.Commands.CreateOrder
{
    public sealed class CreateOrderCommand : IRequest<int>
    {
        public int CustomerId { get; set; }

        public decimal TotalAmount { get; set; }
    }

    public sealed class CreateOrderCommandHandler
        : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IWriteDbContext _writeDbContext;

        public CreateOrderCommandHandler(
            IWriteDbContext writeDbContext)
        {
            _writeDbContext = writeDbContext;
        }

        public async Task<int> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            await _writeDbContext.BeginTransactionAsync(
                cancellationToken);

            try
            {
                // 1. Create Order
                var order = new Domain.Entities.Order(
                    request.CustomerId,
                    request.TotalAmount);

                await _writeDbContext.Orders.AddAsync(
                    order,
                    cancellationToken);

                // 2. Save Order
                await _writeDbContext.SaveChangesAsync(
                    cancellationToken);

                // Order.Id is generated now
                var orderCreatedEvent = new OrderCreatedEvent(
                    order.Id,
                    order.CustomerId,
                    order.TotalAmount,
                    (int)order.Status);

                // 3. Create Outbox Message
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),

                    Type = typeof(OrderCreatedEvent)
                        .FullName!,

                    Payload = JsonSerializer.Serialize(
                        orderCreatedEvent),

                    OccurredOnUtc = DateTime.UtcNow,

                    AggregateId = order.Id.ToString()
                };

                _writeDbContext.OutboxMessages.Add(
                    outboxMessage);

                // 4. Save Outbox
                await _writeDbContext.SaveChangesAsync(
                    cancellationToken);

                // 5. Commit Order + Outbox
                await _writeDbContext.CommitTransactionAsync(
                    cancellationToken);

                return order.Id;
            }
            catch
            {
                await _writeDbContext.RollbackTransactionAsync(
                    cancellationToken);

                throw;
            }
        }
    }
}