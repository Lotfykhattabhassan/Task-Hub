using TaskHub.BuildingBlocks.Application.Abstractions;
using TaskHub.BuildingBlocks.Application.Exceptions;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Entities;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Security;

public sealed class TenantAccessGuard : ITenantAccessGuard
{
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ITenantMembershipRepository _membershipRepository;

    public TenantAccessGuard(
        ICurrentUserAccessor currentUser,
        ITenantMembershipRepository membershipRepository)
    {
        _currentUser = currentUser;
        _membershipRepository = membershipRepository;
    }

    public async Task<TenantMembership?> GetActiveMembershipAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUser.UserId;
        if (userId is null)
            return null;

        var membership = await _membershipRepository.GetByTenantAndUserAsync(
            tenantId,
            userId.Value,
            cancellationToken);

        if (membership is null || membership.Status != MembershipStatus.Active)
            return null;

        return membership;
    }

    public async Task<TenantMembership> EnsureRoleAsync(
        Guid tenantId,
        TenantMemberRole allowedRoles,
        CancellationToken cancellationToken = default)
    {
        var membership = await GetActiveMembershipAsync(tenantId, cancellationToken);

        if (membership is null || (membership.Role & allowedRoles) == 0)
            throw new ForbiddenException(
                "You do not have permission to perform this action on this tenant.");

        return membership;
    }
}
