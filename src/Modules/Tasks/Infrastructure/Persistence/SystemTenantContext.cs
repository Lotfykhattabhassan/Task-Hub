using TaskHub.BuildingBlocks.Application.MultiTenancy;

namespace TaskHub.Modules.Tasks.Infrastructure.Persistence;

// context فاضي بنستخدمه بره الـ request (migrations وإنشاء DB جديدة)
internal sealed class SystemTenantContext : ITenantContext
{
    public Guid TenantId => Guid.Empty;
    public TenantStorageType StorageType => TenantStorageType.Shared;
    public string? ConnectionString => null;
    public bool IsResolved => false;
}
