
using TaskHub.BuildingBlocks.Domain.Exceptions;

namespace TaskHub.Modules.Identity.Application.Exceptions
{
    public class EmailIsExistedException :DomainException
    {
        public EmailIsExistedException() : base("Email is already existed.")
        {
            
        }
    }
}
