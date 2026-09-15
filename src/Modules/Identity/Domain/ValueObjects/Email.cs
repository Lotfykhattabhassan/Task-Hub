using TaskHub.BuildingBlocks.Domain.Exceptions;

namespace TaskHub.Modules.Identity.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email is required.");

        value = value.Trim().ToLowerInvariant();

        if (!value.Contains('@'))
            throw new DomainException("Invalid email format.");

        return new Email(value);
    }

    public override string ToString()
    {
        return Value;
    }
}