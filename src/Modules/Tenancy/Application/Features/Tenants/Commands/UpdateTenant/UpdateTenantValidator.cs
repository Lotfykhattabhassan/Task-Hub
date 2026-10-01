using FluentValidation;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.UpdateTenant;

public sealed class UpdateTenantValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
