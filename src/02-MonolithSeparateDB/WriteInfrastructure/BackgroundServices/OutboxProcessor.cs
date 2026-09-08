using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SimpleSeparateDb.Api.Application.Abstractions.Persistence;
using SimpleSeparateDb.Api.Application.Events;
using System.Text.Json;

namespace SimpleSeparateDb.Api.WriteInfrastructure.BackgroundServices
{
    public sealed class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutboxProcessor(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =
                        _scopeFactory.CreateScope();

                    var writeDbContext =
                        scope.ServiceProvider
                            .GetRequiredService<IWriteDbContext>();

                    var readDbContext =
                        scope.ServiceProvider
                            .GetRequiredService<IReadDbContext>();

                    var messages =
                        await writeDbContext.OutboxMessages
                            .Where(x => x.ProcessedOnUtc == null)
                            .OrderBy(x => x.OccurredOnUtc)
                            .Take(20)
                            .ToListAsync(stoppingToken);

                    foreach (var message in messages)
                    {
                        try
                        {
                            if (message.Type ==
                                typeof(OrderCreatedEvent).FullName)
                            {
                                var orderCreatedEvent =
                                    JsonSerializer.Deserialize<OrderCreatedEvent>(
                                        message.Payload);

                                if (orderCreatedEvent is null)
                                    throw new InvalidOperationException(
                                        "Could not deserialize OrderCreatedEvent.");

                                var existingOrder =
                                    await readDbContext.FindAsync(
                                        orderCreatedEvent.OrderId,
                                        stoppingToken);

                                if (existingOrder is null)
                                {
                                    var readModel = new Domain.Models.OrderReadModel
                                    {
                                        Id = orderCreatedEvent.OrderId,
                                        CustomerId = orderCreatedEvent.CustomerId,
                                        TotalAmount = orderCreatedEvent.TotalAmount,
                                        Status = orderCreatedEvent.Status
                                    };

                                    await readDbContext.AddAsync(
                                        readModel,
                                        stoppingToken);

                                    await readDbContext.SaveChangesAsync(
                                        stoppingToken);
                                }

                                message.ProcessedOnUtc =
                                    DateTime.UtcNow;

                                message.Error = null;

                                await writeDbContext.SaveChangesAsync(
                                    stoppingToken);
                            }
                        }
                        catch (Exception ex)
                        {
                            message.Error = ex.Message;

                            await writeDbContext.SaveChangesAsync(
                                stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Outbox processing error: {ex.Message}");
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(5),
                    stoppingToken);
            }
        }
    }
}