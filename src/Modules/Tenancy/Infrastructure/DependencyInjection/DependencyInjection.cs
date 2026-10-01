using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.BuildingBlocks.Infrastructure.MultiTenancy;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Infrastructure.Persistence;
using TaskHub.Modules.Tenancy.Infrastructure.Repositories;
using TaskHub.Modules.Tenancy.Infrastructure.Security;

namespace TaskHub.Modules.Tenancy.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddTenancyInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTenantInfrastructure();

        services.AddDbContext<TenantRegistryDbContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("TaskHub")
                ?? throw new InvalidOperationException(
                    "Central TaskHub connection string is missing.");

            options.UseSqlServer(connectionString);
        });

        // مفاتيح التشفير: في الـ production لازم تتخزن في مكان ثابت
        // (PersistKeysToFileSystem أو Azure Blob ...) وإلا الـ connection strings
        // المشفرة مش هتتفك بعد إعادة النشر
        services.AddDataProtection()
            .SetApplicationName("TaskHub");

        services.AddSingleton<IConnectionStringProtector, ConnectionStringProtector>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantMembershipRepository,
            TenantMembershipRepository>();

        return services;
    }
}
