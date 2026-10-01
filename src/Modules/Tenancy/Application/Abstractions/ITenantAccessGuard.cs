using TaskHub.Modules.Tenancy.Domain.Entities;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Abstractions;

public interface ITenantAccessGuard
{
    // عضوية المستخدم الحالي في الـ tenant لو Active، وإلا null
    Task<TenantMembership?> GetActiveMembershipAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);

    // بيرمي ForbiddenException لو المستخدم مش عضو Active بواحد من الـ roles المسموحة
    Task<TenantMembership> EnsureRoleAsync(
        Guid tenantId,
        TenantMemberRole allowedRoles,
        CancellationToken cancellationToken = default);
}
