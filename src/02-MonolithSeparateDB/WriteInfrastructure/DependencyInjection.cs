using Microsoft.EntityFrameworkCore;
using SimpleSeparateDb.Api.Application.Abstractions.Persistence;
using SimpleSeparateDb.Api.WriteInfrastructure.Persistence;

namespace SimpleSeparateDb.Api.WriteInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWriteInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("WriteConnection")
                ?? throw new InvalidOperationException(
                    "WriteConnection is not configured.");

            services.AddDbContext<WriteDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IWriteDbContext>(
                provider => provider.GetRequiredService<WriteDbContext>());

            return services;
        }
    }
}
