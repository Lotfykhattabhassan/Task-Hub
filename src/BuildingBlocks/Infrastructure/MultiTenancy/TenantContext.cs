using TaskHub.BuildingBlocks.Application.MultiTenancy;

namespace TaskHub.BuildingBlocks.Infrastructure.MultiTenancy;

public sealed class TenantContext : ITenantContext, ITenantContextSetter
{
    public Guid TenantId { get; private set; }
    public TenantStorageType StorageType { get; private set; }
    public string? ConnectionString { get; private set; }
    public bool IsResolved { get; private set; }

    public void SetTenantContext(
        Guid tenantId,
        TenantStorageType storageType,
        string? connectionString)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));

        if (storageType == TenantStorageType.Dedicated &&
            string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Dedicated tenant must have a connection string.");
        }

        if (storageType == TenantStorageType.Shared &&
            !string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Shared tenant cannot have a dedicated connection string.");
        }

        TenantId = tenantId;
        StorageType = storageType;
        ConnectionString = connectionString;
        IsResolved = true;
    }
}
