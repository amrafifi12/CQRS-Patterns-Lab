using Microsoft.EntityFrameworkCore;
using SimpleSeparateDb.Api.Application.Abstractions.Persistence;
using SimpleSeparateDb.Api.Domain.Models;

namespace SimpleSeparateDb.Api.ReadInfrastructure.Persistence
{
    public class ReadDbContext : DbContext, IReadDbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }

        public DbSet<OrderReadModel> OrderReadModels { get; set; }

        public async Task<OrderReadModel?> FindAsync(int id, CancellationToken cancellationToken)
        {
            return await OrderReadModels.FindAsync(new object[] { id }, cancellationToken);
        }
        public async Task AddAsync(
                            OrderReadModel order,
                            CancellationToken cancellationToken)
        {
            await OrderReadModels.AddAsync(
                order,
                cancellationToken);
        }

        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderReadModel>(entity =>
            {
                entity.ToTable("OrderReadModels");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CustomerId);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.Status).IsRequired();
                entity.HasKey(e => e.Id);
            });
        }
    }
}
