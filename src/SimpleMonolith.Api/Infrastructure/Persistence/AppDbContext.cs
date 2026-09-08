using Microsoft.EntityFrameworkCore;
using SimpleMonolith.Api.Application.Abstractions.Persistence;

namespace SimpleMonolith.Api.Infrastructure.Persistence
{
    public class AppDbContext:DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Order> Orders { get; set; }

    }
}
