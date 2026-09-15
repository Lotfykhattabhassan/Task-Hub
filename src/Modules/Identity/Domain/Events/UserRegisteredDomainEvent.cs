using TaskHub.BuildingBlocks.Domain.Events;

namespace TaskHub.Modules.Identity.Domain.Events;

public sealed class UserRegisteredDomainEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserRegisteredDomainEvent(Guid userId)
    {
        UserId = userId;
    }
}