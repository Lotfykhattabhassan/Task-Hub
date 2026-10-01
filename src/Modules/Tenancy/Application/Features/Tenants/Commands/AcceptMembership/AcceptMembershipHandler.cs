using MediatR;
using TaskHub.BuildingBlocks.Application.Abstractions;
using TaskHub.BuildingBlocks.Application.Exceptions;
using TaskHub.Modules.Tenancy.Application.Abstractions;

namespace TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AcceptMembership;

public sealed class AcceptMembershipHandler : IRequestHandler<AcceptMembershipCommand>
{
    private readonly ITenantMembershipRepository _membershipRepository;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptMembershipHandler(
        ITenantMembershipRepository membershipRepository,
        ICurrentUserAccessor currentUser,
        IUnitOfWork unitOfWork)
    {
        _membershipRepository = membershipRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        AcceptMembershipCommand command,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenException("User is not authenticated.");

        var membership = await _membershipRepository.GetByTenantAndUserAsync(
                command.TenantId,
                userId,
                cancellationToken)
            ?? throw new NotFoundException("Invitation was not found.");

        membership.Accept();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
