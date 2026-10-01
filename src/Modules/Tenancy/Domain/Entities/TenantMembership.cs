using TaskHub.BuildingBlocks.Domain.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Domain.Entities;

public class TenantMembership : Entity<Guid>
{
    public Guid UserId { get; private set; }

    public Guid TenantId { get; private set; }

    public TenantMemberRole Role { get; private set; }

    public MembershipStatus Status { get; private set; }

    public DateTime JoinedAt { get; private set; }
    public Tenant Tenant { get; private set; }

    private TenantMembership() { }

    private TenantMembership(
        Guid userId,
        Guid tenantId,
        TenantMemberRole role)
        : base(Guid.NewGuid())
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID cannot be empty.",
                nameof(userId));
        }

        if (tenantId == Guid.Empty)
        {
            throw new ArgumentException(
                "Tenant ID cannot be empty.",
                nameof(tenantId));
        }

        if (!Enum.IsDefined(role))
        {
            throw new ArgumentOutOfRangeException(nameof(role));
        }

        UserId = userId;
        TenantId = tenantId;
        Role = role;
        Status = MembershipStatus.Pending;
        JoinedAt = DateTime.UtcNow;
    }

    public static TenantMembership Create(
        Guid userId,
        Guid tenantId,
        TenantMemberRole role = TenantMemberRole.Member)
    {
        return new TenantMembership(
            userId,
            tenantId,
            role);
    }

    public void Accept()
    {
        if (Status != MembershipStatus.Pending)
            return;

        Status = MembershipStatus.Active;
    }

    public void Reactivate()
    {
        if (Status != MembershipStatus.Suspended)
            return;

        Status = MembershipStatus.Active;
    }

    public void ChangeRole(TenantMemberRole role)
    {
        if (!Enum.IsDefined(role))
        {
            throw new ArgumentOutOfRangeException(nameof(role));
        }

        if (Role == role)
            return;

        Role = role;
    }

    public void Suspend()
    {
        if (Status != MembershipStatus.Active)
            return;

        Status = MembershipStatus.Suspended;
    }

    public void Revoke()
    {
        if (Status == MembershipStatus.Revoked)
            return;

        Status = MembershipStatus.Revoked;
    }
}