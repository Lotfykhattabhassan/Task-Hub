using TaskHub.Modules.Tenancy.Domain.Entities;

namespace TaskHub.Modules.Tenancy.Application.Abstractions;

public interface ITenantMembershipRepository
{
    Task<TenantMembership?> GetByIdAsync(
        Guid membershipId,
        CancellationToken cancellationToken = default);

    Task<TenantMembership?> GetByTenantAndUserAsync(
        Guid tenantId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TenantMembership>> GetByTenantIdAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid tenantId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        TenantMembership membership,
        CancellationToken cancellationToken = default);
}