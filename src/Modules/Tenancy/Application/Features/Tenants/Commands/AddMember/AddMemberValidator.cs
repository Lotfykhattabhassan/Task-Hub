using FluentValidation;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AddMember;

public sealed class AddMemberValidator : AbstractValidator<AddMemberCommand>
{
    public AddMemberValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();

        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Role)
            .Must(role => Enum.IsDefined(role) && role != TenantMemberRole.Owner)
            .WithMessage("Role is invalid. The Owner role cannot be assigned to an invited member.");
    }
}
