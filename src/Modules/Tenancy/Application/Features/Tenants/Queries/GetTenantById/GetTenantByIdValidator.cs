using FluentValidation;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenantById;

public sealed class GetTenantByIdValidator : AbstractValidator<GetTenantByIdQuery>
{
    public GetTenantByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
