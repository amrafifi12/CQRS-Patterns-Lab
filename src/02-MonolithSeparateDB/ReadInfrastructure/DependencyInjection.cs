using Microsoft.EntityFrameworkCore;
using SimpleSeparateDb.Api.Application.Abstractions.Persistence;
using SimpleSeparateDb.Api.ReadInfrastructure.Persistence;

namespace SimpleSeparateDb.Api.ReadInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddReadInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("ReadConnection")
                ?? throw new InvalidOperationException(
                    "ReadConnection is not configured.");

            services.AddDbContext<ReadDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IReadDbContext>(
                provider => provider.GetRequiredService<ReadDbContext>());

            return services;
        }
    }
}
