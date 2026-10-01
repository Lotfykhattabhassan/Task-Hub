using Microsoft.EntityFrameworkCore;
using TaskHub.Modules.Tenancy.Application.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Entities;
using TaskHub.Modules.Tenancy.Infrastructure.Persistence;

namespace TaskHub.Modules.Tenancy.Infrastructure.Repositories;

public sealed class TenantMembershipRepository
    : ITenantMembershipRepository
{
    private readonly TenantRegistryDbContext _context;

    public TenantMembershipRepository(TenantRegistryDbContext context)
    {
        _context = context;
    }

    public async Task<TenantMembership?> GetByIdAsync(
        Guid membershipId,
        CancellationToken cancellationToken = default)
    {
        return await _context.TenantMemberships
            .FirstOrDefaultAsync(
                x => x.Id == membershipId,
                cancellationToken);
    }

    public async Task<TenantMembership?> GetByTenantAndUserAsync(
        Guid tenantId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.TenantMemberships
            .FirstOrDefaultAsync(
                x => x.TenantId == tenantId &&
                     x.UserId == userId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<TenantMembership>> GetByTenantIdAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.TenantMemberships
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid tenantId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.TenantMemberships
            .AnyAsync(
                x => x.TenantId == tenantId &&
                     x.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        TenantMembership membership,
        CancellationToken cancellationToken = default)
    {
        await _context.TenantMemberships.AddAsync(
            membership,
            cancellationToken);
    }
}