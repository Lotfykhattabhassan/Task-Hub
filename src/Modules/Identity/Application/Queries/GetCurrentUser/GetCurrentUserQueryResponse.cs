
using TaskHub.Modules.Identity.Domain.Enums;

namespace TaskHub.Modules.Identity.Application.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = null!;
        public UserStatus Status { get; set; }
    }
}
