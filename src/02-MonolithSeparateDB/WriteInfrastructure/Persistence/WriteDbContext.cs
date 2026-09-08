using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleSeparateDb.Api.Application.Abstractions.Persistence;
using SimpleSeparateDb.Api.Domain.Entities;

namespace SimpleSeparateDb.Api.WriteInfrastructure.Persistence
{
    public class WriteDbContext : DbContext, IWriteDbContext
    {
        private IDbContextTransaction? _transaction;

        public WriteDbContext(
            DbContextOptions<WriteDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OutboxMessage> OutboxMessages
            => Set<OutboxMessage>();

        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(
            CancellationToken cancellationToken)
        {
            _transaction = await Database.BeginTransactionAsync(
                cancellationToken);
        }

        public async Task CommitTransactionAsync(
            CancellationToken cancellationToken)
        {
            if (_transaction is null)
                throw new InvalidOperationException(
                    "Transaction has not been started.");

            await _transaction.CommitAsync(cancellationToken);

            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackTransactionAsync(
            CancellationToken cancellationToken)
        {
            if (_transaction is null)
                return;

            await _transaction.RollbackAsync(cancellationToken);

            await _transaction.DisposeAsync();

            _transaction = null;
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CustomerId);

                entity.Property(e => e.TotalAmount)
                    .HasPrecision(18, 2);

                entity.Property(e => e.Status)
                    .HasConversion<int>()
                    .IsRequired();
            });

            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Type)
                    .IsRequired();

                entity.Property(x => x.Payload)
                    .IsRequired();

                entity.Property(x => x.OccurredOnUtc)
                    .IsRequired();

                entity.Property(x => x.AggregateId)
                    .IsRequired();

                entity.HasIndex(x => x.ProcessedOnUtc);
            });
        }
    }
}