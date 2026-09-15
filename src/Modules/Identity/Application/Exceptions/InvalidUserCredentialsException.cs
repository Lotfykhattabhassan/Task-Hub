
using TaskHub.BuildingBlocks.Domain.Exceptions;

namespace TaskHub.Modules.Identity.Application.Exceptions
{
    public class InvalidUserCredentialsException : DomainException
    {
        public InvalidUserCredentialsException() 
            : base("Invalid user credentials.")
        {
            
        }
    }
}
