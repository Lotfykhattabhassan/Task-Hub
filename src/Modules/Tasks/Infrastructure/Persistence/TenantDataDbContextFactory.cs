using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskHub.Modules.Tasks.Infrastructure.Persistence;

public sealed class TenantDataDbContextFactory : IDesignTimeDbContextFactory<TenantDataDbContext>
{
    public TenantDataDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("TASKHUB_CONNECTION")
            ?? "Server=.;Database=TaskHubDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        var builder = new DbContextOptionsBuilder<TenantDataDbContext>();
        builder.UseTenantDataSqlServer(connectionString);

        return new TenantDataDbContext(builder.Options, new SystemTenantContext());
    }
}
