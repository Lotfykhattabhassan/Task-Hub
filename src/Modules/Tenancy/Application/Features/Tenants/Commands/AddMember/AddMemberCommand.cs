using MediatR;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AddMember;

public sealed record AddMemberCommand(
    Guid TenantId,
    Guid UserId,
    TenantMemberRole Role) : IRequest<AddMemberResponse>;
