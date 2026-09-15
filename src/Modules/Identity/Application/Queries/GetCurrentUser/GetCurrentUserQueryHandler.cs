using TaskHub.BuildingBlocks.Application.Abstractions.Messaging;
using TaskHub.Modules.Identity.Application.Abstractions.Identity;
using TaskHub.Modules.Identity.Application.Exceptions;

namespace TaskHub.Modules.Identity.Application.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, GetCurrentUserQueryResponse>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityService _identityService;
        public GetCurrentUserQueryHandler(ICurrentUser currentUser, IIdentityService identityService)
        {
            _currentUser = currentUser;
            _identityService = identityService;
        }
        public async Task<GetCurrentUserQueryResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var id = _currentUser.UserId;

            if (id == null)
                throw new InvalidUserCredentialsException();


            var currentUser = await _identityService.FindByIdAsync(id.Value, cancellationToken);

            if (currentUser == null)
                throw new UserNotFoundException();

            return new GetCurrentUserQueryResponse
            {
                UserId = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email.Value,
                Status = currentUser.Status
            };
        }
    }
}
