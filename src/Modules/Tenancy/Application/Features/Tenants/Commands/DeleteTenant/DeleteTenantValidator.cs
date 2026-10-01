using FluentValidation;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.DeleteTenant;

public sealed class DeleteTenantValidator : AbstractValidator<DeleteTenantCommand>
{
    public DeleteTenantValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
