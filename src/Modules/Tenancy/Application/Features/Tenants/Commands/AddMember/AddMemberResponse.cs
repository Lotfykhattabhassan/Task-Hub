using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AddMember;

public sealed record AddMemberResponse(
    Guid MembershipId,
    Guid TenantId,
    Guid UserId,
    TenantMemberRole Role,
    MembershipStatus Status);
