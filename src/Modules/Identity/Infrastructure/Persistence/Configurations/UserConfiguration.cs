using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskHub.Modules.Identity.Domain.Entities;
using TaskHub.Modules.Identity.Domain.Enums;
using TaskHub.Modules.Identity.Domain.ValueObjects;
using TaskHub.Modules.Identity.Infrastructure.Identity;

namespace TaskHub.Modules.Identity.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .IsRequired();

        builder.Property(user => user.Status)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<UserStatus>(value))
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<ApplicationUser>(applicationUser => applicationUser.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
