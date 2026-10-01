using Microsoft.Extensions.Configuration;
using TaskHub.BuildingBlocks.Application.MultiTenancy;

namespace TaskHub.BuildingBlocks.Infrastructure.MultiTenancy;

public sealed class TenantConnectionStringResolver
    : ITenantConnectionStringResolver
{
    private readonly ITenantContext _tenantContext;
    private readonly IConfiguration _configuration;

    public TenantConnectionStringResolver(
        ITenantContext tenantContext,
        IConfiguration configuration)
    {
        _tenantContext = tenantContext;
        _configuration = configuration;
    }

    public string Resolve()
    {
        if (!_tenantContext.IsResolved)
        {
            throw new InvalidOperationException(
                "Tenant context has not been resolved.");
        }

        return _tenantContext.StorageType switch
        {
            TenantStorageType.Shared =>
                _configuration.GetConnectionString("TaskHub")
                ?? throw new InvalidOperationException(
                    "Shared tenant connection string is missing."),

            TenantStorageType.Dedicated =>
                _tenantContext.ConnectionString
                ?? throw new InvalidOperationException(
                    "Dedicated tenant connection string is missing."),

            _ => throw new InvalidOperationException(
                "Unsupported tenant storage type.")
        };
    }
}
