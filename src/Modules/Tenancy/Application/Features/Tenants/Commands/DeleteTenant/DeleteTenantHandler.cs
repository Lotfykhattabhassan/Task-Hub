using MediatR;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.DeleteTenant;

public sealed class DeleteTenantHandler : IRequestHandler<DeleteTenantCommand, DeleteTenantResponse?>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantAccessGuard _accessGuard;

    public DeleteTenantHandler(
        ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        ITenantAccessGuard accessGuard)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _accessGuard = accessGuard;
    }

    public async Task<DeleteTenantResponse?> Handle(
        DeleteTenantCommand command,
        CancellationToken cancellationToken)
    {
        // Owner بس
        await _accessGuard.EnsureRoleAsync(
            command.Id,
            TenantMemberRole.Owner,
            cancellationToken);

        var tenant = await _tenantRepository.GetByIdAsync(command.Id, cancellationToken);
        if (tenant is null)
            return null;

        tenant.MarkAsDeleted();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DeleteTenantResponse(tenant.Id);
    }
}
