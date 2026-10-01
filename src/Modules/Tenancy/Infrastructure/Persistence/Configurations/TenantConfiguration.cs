using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskHub.Modules.Tenancy.Domain.Entities;
using TaskHub.Modules.Tenancy.Domain.ValueObjects;

namespace TaskHub.Modules.Tenancy.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(x => x.Id);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(x => x.Name)
            .HasConversion(
                name => name.Value,
                value => TenantName.Create(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasConversion(
                slug => slug.Value,
                value => TenantSlug.Create(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Slug)
            .IsUnique();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.StorageMode)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.ConnectionString)
            .HasMaxLength(2000);

        builder.HasMany(x => x.Members)
            .WithOne(x=>x.Tenant)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}