using Microsoft.EntityFrameworkCore;
using SimpleMonolith.Api.Application.Abstractions.Persistence;
using SimpleMonolith.Api.Infrastructure.Persistence;

namespace SimpleMonolith.Api.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationDbContext>(
                provider => provider.GetRequiredService<AppDbContext>());

            return services;
        }
    }
}
