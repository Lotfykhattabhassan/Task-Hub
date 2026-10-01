using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskHub.Modules.Tenancy.Domain.Entities;

namespace TaskHub.Modules.Tenancy.Infrastructure.Persistence.Configurations;

public class TenantMembershipConfiguration
    : IEntityTypeConfiguration<TenantMembership>
{
    public void Configure(EntityTypeBuilder<TenantMembership> builder)
    {
        builder.ToTable("TenantMemberships");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.JoinedAt)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.TenantId,
            x.UserId
        })
        .IsUnique();
    }
}