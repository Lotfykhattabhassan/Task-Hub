using MediatR;
using TaskHub.BuildingBlocks.Application.Exceptions;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Enums;
using TaskHub.Modules.Tenancy.Domain.ValueObjects;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.UpdateTenant;

public sealed class UpdateTenantHandler : IRequestHandler<UpdateTenantCommand, UpdateTenantResponse?>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantAccessGuard _accessGuard;

    public UpdateTenantHandler(
        ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        ITenantAccessGuard accessGuard)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _accessGuard = accessGuard;
    }

    public async Task<UpdateTenantResponse?> Handle(
        UpdateTenantCommand command,
        CancellationToken cancellationToken)
    {
        // Owner أو Admin بس
        await _accessGuard.EnsureRoleAsync(
            command.Id,
            TenantMemberRole.Owner | TenantMemberRole.Admin,
            cancellationToken);

        var tenant = await _tenantRepository.GetByIdAsync(command.Id, cancellationToken);
        if (tenant is null)
            return null;

        var newSlug = TenantSlug.Create(command.Slug.Trim());

        if (newSlug.Value != tenant.Slug.Value &&
            await _tenantRepository.ExistsBySlugAsync(newSlug.Value, cancellationToken))
        {
            throw new ConflictException($"Tenant slug '{newSlug.Value}' already exists.");
        }

        tenant.Rename(TenantName.Create(command.Name.Trim()));

        tenant.ChangeSlug(newSlug);

        if (command.Status == TenantStatus.Active)
            tenant.Activate();

        if (command.Status == TenantStatus.Suspended)
            tenant.Suspend();

        if (command.Status == TenantStatus.Inactive)
            tenant.Deactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateTenantResponse(
            tenant.Id,
            tenant.Name.Value,
            tenant.Slug.Value,
            tenant.StorageMode,
            tenant.Status);
    }
}
