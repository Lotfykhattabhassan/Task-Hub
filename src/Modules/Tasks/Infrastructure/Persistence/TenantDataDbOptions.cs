using Microsoft.EntityFrameworkCore;

namespace TaskHub.Modules.Tasks.Infrastructure.Persistence;

public static class TenantDataDbOptions
{
    // نفس الإعدادات في الـ DI وفي الـ provisioner وفي الـ design-time factory
    public static DbContextOptionsBuilder UseTenantDataSqlServer(
        this DbContextOptionsBuilder options,
        string connectionString)
    {
        return options.UseSqlServer(
            connectionString,
            sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "tasks"));
    }
}
