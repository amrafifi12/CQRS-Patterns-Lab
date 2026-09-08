using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SimpleSeparateDb.Api.WriteInfrastructure.Persistence
{
    public class WriteDbContextFactory : IDesignTimeDbContextFactory<WriteDbContext>
    {
        public WriteDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("WriteConnection");
            var optionsBuilder = new DbContextOptionsBuilder<WriteDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WriteDbContext(optionsBuilder.Options);
        }
    }
}
