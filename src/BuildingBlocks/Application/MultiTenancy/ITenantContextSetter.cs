namespace TaskHub.BuildingBlocks.Application.MultiTenancy;

public interface ITenantContextSetter
{
    void SetTenantContext(
        Guid tenantId,
        TenantStorageType storageType,
        string? connectionString);
}
