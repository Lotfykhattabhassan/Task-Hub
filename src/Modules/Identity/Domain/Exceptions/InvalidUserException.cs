
using TaskHub.BuildingBlocks.Domain.Exceptions;

namespace TaskHub.Modules.Identity.Domain.Exceptions
{
    public class InvalidUserException : DomainException
    {
        public InvalidUserException() : base("The user is invalid.")
        {
            
        }
    }
}
