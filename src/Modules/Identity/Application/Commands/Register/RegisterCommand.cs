using TaskHub.BuildingBlocks.Application.Abstractions.Messaging;
using TaskHub.Modules.Identity.Domain.ValueObjects;

namespace TaskHub.Modules.Identity.Application.Commands.Register
{
    public sealed record RegisterCommand(string email,
        string firstname,
        string lastname,
        string phoneNumber,
        string password) : ICommand<RegisterCommandResponse>;
    
}
