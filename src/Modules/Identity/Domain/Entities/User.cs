using TaskHub.BuildingBlocks.Domain.Abstractions;
using TaskHub.Modules.Identity.Domain.Enums;
using TaskHub.Modules.Identity.Domain.Events;
using TaskHub.Modules.Identity.Domain.ValueObjects;

namespace TaskHub.Modules.Identity.Domain.Entities;

public class User : AggregateRoot<Guid>
{
    public Email Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserStatus Status { get; private set; }

    private User(Guid id, Email email, string firstName, string lastName)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException(nameof(firstName));
        FirstName = firstName;

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException(nameof(lastName));

        LastName = lastName;

        Email = email;
        Status = UserStatus.Active;
    }

    public static User Create(Email email , string firstName,string lastName)
    {
        var user = new User(Guid.NewGuid(), email,firstName, lastName);

        user.AddDomainEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
        MarkAsUpdated();
    }

    public void Activate()
    {
        if (Status == UserStatus.Active)
            return;

        Status = UserStatus.Active;
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        if (Status == UserStatus.Inactive)
            return;

        Status = UserStatus.Inactive;
        MarkAsUpdated();
    }
}
