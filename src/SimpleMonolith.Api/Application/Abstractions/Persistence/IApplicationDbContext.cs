using Microsoft.EntityFrameworkCore;

namespace SimpleMonolith.Api.Application.Abstractions.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Order> Orders { get; }

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken);
    }
}
