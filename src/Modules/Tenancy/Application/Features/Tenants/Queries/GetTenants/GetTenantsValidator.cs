using FluentValidation;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenants;

public sealed class GetTenantsValidator : AbstractValidator<GetTenantsQuery>
{
    public GetTenantsValidator()
    {
        // No request parameters to validate.
    }
}
