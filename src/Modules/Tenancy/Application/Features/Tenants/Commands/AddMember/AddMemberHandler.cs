using MediatR;
using TaskHub.BuildingBlocks.Application.Exceptions;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Entities;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AddMember;

public sealed class AddMemberHandler : IRequestHandler<AddMemberCommand, AddMemberResponse>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantMembershipRepository _membershipRepository;
    private readonly ITenantAccessGuard _accessGuard;
    private readonly IUnitOfWork _unitOfWork;

    public AddMemberHandler(
        ITenantRepository tenantRepository,
        ITenantMembershipRepository membershipRepository,
        ITenantAccessGuard accessGuard,
        IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _membershipRepository = membershipRepository;
        _accessGuard = accessGuard;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddMemberResponse> Handle(
        AddMemberCommand command,
        CancellationToken cancellationToken)
    {
        // Owner أو Admin بس
        await _accessGuard.EnsureRoleAsync(
            command.TenantId,
            TenantMemberRole.Owner | TenantMemberRole.Admin,
            cancellationToken);

        var tenant = await _tenantRepository.GetByIdAsync(command.TenantId, cancellationToken)
            ?? throw new NotFoundException("Tenant was not found.");

        if (await _membershipRepository.ExistsAsync(tenant.Id, command.UserId, cancellationToken))
            throw new ConflictException("User is already a member of this tenant.");

        var membership = TenantMembership.Create(command.UserId, tenant.Id, command.Role);

        await _membershipRepository.AddAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddMemberResponse(
            membership.Id,
            membership.TenantId,
            membership.UserId,
            membership.Role,
            membership.Status);
    }
}
