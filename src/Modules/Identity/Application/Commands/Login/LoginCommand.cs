
using TaskHub.BuildingBlocks.Application.Abstractions.Messaging;

namespace TaskHub.Modules.Identity.Application.Commands.Login
{
    public sealed record LoginCommand(string email, string password) : ICommand<LoginCommandResponse>;
    
}
