using TaskHub.BuildingBlocks.Application.Abstractions.Messaging;
using TaskHub.Modules.Identity.Application.Abstractions.Identity;
using TaskHub.Modules.Identity.Application.Exceptions;
using TaskHub.Modules.Identity.Domain.ValueObjects;

namespace TaskHub.Modules.Identity.Application.Commands.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public LoginCommandHandler(IIdentityService identityService,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _identityService = identityService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginCommandResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var email = Email.Create(request.email);

            var result = await _identityService.ValidateCredentialsAsync(email, request.password);

            if (result == null)
                throw new UserNotFoundException();

            var token = _jwtTokenGenerator.GenerateToken(result.UserId, result.Email);

            return new LoginCommandResponse
            {
                AccessToken = token.AccessToken,
                ExpiredAt = token.ExpiresAt
            };
        }
    }
}
