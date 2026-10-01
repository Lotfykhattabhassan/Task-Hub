using Microsoft.EntityFrameworkCore;
using TaskHub.BuildingBlocks.Application.MultiTenancy;

namespace TaskHub.Modules.Tasks.Infrastructure.Persistence;

// بيطبق migrations الـ Tasks على DB جديدة (tenant من نوع Dedicated)
public sealed class TasksDatabaseProvisioner : ITenantDatabaseProvisioner
{
    public async Task ProvisionAsync(
        string connectionString,
        CancellationToken cancellationToken = default)
    {
        var builder = new DbContextOptionsBuilder<TenantDataDbContext>();
        builder.UseTenantDataSqlServer(connectionString);

        await using var context = new TenantDataDbContext(builder.Options, new SystemTenantContext());
        await context.Database.MigrateAsync(cancellationToken);
    }
}
