using TaskHub.BuildingBlocks.Application.Abstractions.Messaging;
using TaskHub.Modules.Identity.Application.Abstractions.Identity;
using TaskHub.Modules.Identity.Application.Abstractions.Persistence;
using TaskHub.Modules.Identity.Application.Exceptions;
using TaskHub.Modules.Identity.Domain.Entities;
using TaskHub.Modules.Identity.Domain.ValueObjects;

namespace TaskHub.Modules.Identity.Application.Commands.Register;

public class RegisterCommandHandler
    : ICommandHandler<RegisterCommand, RegisterCommandResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IIdentityUnitOfWork _identityUnitOfWork;

    public RegisterCommandHandler(
        IIdentityService identityService,
        IIdentityUnitOfWork identityUnitOfWork)
    {
        _identityService = identityService;
        _identityUnitOfWork = identityUnitOfWork;
    }

    public async Task<RegisterCommandResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(request.email);

        var user = User.Create(email, request.firstname, request.lastname);

        var result = await _identityService.CreateUserAsync(user,
            request.phoneNumber,
            request.password);

        if (!result.Succeeded)
            throw new EmailIsExistedException();

        await _identityUnitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterCommandResponse
        {
            Message = "User registered successfully."
        };
    }
}