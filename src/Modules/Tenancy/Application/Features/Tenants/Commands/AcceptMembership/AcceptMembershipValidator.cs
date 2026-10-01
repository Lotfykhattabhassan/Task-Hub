using FluentValidation;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AcceptMembership;

public sealed class AcceptMembershipValidator : AbstractValidator<AcceptMembershipCommand>
{
    public AcceptMembershipValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
    }
}
