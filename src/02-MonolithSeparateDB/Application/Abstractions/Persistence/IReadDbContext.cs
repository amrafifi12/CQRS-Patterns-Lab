using SimpleSeparateDb.Api.Domain.Models;

public interface IReadDbContext
{
    Task<OrderReadModel?> FindAsync(
        int id,
        CancellationToken cancellationToken);

    Task AddAsync(
        OrderReadModel order,
        CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}