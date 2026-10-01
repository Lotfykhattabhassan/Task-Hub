using MediatR;
using TaskHub.Modules.Tenancy.Application.Abstractions;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenantById;

public sealed class GetTenantByIdHandler : IRequestHandler<GetTenantByIdQuery, GetTenantByIdResponse?>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantAccessGuard _accessGuard;

    public GetTenantByIdHandler(
        ITenantRepository tenantRepository,
        ITenantAccessGuard accessGuard)
    {
        _tenantRepository = tenantRepository;
        _accessGuard = accessGuard;
    }

    public async Task<GetTenantByIdResponse?> Handle(
        GetTenantByIdQuery query,
        CancellationToken cancellationToken)
    {
        // لو المستخدم مش عضو نرجّع null (404) عشان منكشفش إن الـ tenant موجود
        var membership = await _accessGuard.GetActiveMembershipAsync(query.Id, cancellationToken);
        if (membership is null)
            return null;

        var tenant = await _tenantRepository.GetByIdAsync(query.Id, cancellationToken);
        if (tenant is null)
            return null;

        return new GetTenantByIdResponse(
            tenant.Id,
            tenant.Name.Value,
            tenant.Slug.Value,
            tenant.StorageMode,
            tenant.Status);
    }
}
