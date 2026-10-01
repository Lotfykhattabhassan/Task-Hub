using Microsoft.EntityFrameworkCore;
using TaskHub.Modules.Tenancy.Domain.Entities;

namespace TaskHub.Modules.Tenancy.Infrastructure.Persistence;

public class TenantRegistryDbContext : DbContext
{
    public TenantRegistryDbContext(
        DbContextOptions<TenantRegistryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    public DbSet<TenantMembership> TenantMemberships => Set<TenantMembership>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TenantRegistryDbContext).Assembly);
    }
}