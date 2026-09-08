using Microsoft.EntityFrameworkCore;
using SimpleSeparateDb.Api.Domain.Entities;
using SimpleSeparateDb.Api.WriteInfrastructure.Persistence;

namespace SimpleSeparateDb.Api.Application.Abstractions.Persistence
{
    public interface IWriteDbContext
    {
        DbSet<Order> Orders { get; }

        DbSet<OutboxMessage> OutboxMessages { get; }

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken);

        Task BeginTransactionAsync(
            CancellationToken cancellationToken);

        Task CommitTransactionAsync(
            CancellationToken cancellationToken);

        Task RollbackTransactionAsync(
            CancellationToken cancellationToken);
    }
}