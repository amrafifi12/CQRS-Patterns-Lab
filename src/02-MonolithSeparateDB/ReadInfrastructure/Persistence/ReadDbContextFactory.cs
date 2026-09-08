using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SimpleSeparateDb.Api.ReadInfrastructure.Persistence
{
    public class ReadDbContextFactory : IDesignTimeDbContextFactory<ReadDbContext>
    {
        public ReadDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("ReadConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ReadDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ReadDbContext(optionsBuilder.Options);
        }
    }
}
