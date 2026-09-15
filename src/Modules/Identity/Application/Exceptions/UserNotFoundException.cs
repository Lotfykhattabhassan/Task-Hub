

using TaskHub.BuildingBlocks.Domain.Exceptions;

namespace TaskHub.Modules.Identity.Application.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException()
            : base("User not found.")
        {
        }
    }
}
