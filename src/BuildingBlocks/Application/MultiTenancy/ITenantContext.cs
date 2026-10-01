namespace TaskHub.BuildingBlocks.Application.MultiTenancy;

public interface ITenantContext
{
    Guid TenantId { get; }
    TenantStorageType StorageType { get; }
    string? ConnectionString { get; }
    bool IsResolved { get; }
}
