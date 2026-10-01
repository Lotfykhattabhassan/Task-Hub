using FluentValidation;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.CreateTenant;

public sealed class CreateTenantValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.StorageMode)
            .IsInEnum();

        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .When(x => x.StorageMode == TenantStorageMode.Dedicated)
            .WithMessage("ConnectionString is required for dedicated storage.");
    }
}
