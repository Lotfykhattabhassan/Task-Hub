namespace TaskHub.Modules.Tenancy.Domain.ValueObjects;

public sealed record TenantName
{
    public string Value { get; }

    private TenantName(string value)
    {
        Value = value;
    }

    public static TenantName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Tenant name cannot be empty.",
                nameof(value));

        value = value.Trim();

        if (value.Length < 2)
            throw new ArgumentException(
                "Tenant name must be at least 2 characters.",
                nameof(value));

        if (value.Length > 100)
            throw new ArgumentException(
                "Tenant name cannot exceed 100 characters.",
                nameof(value));

        return new TenantName(value);
    }

    public override string ToString() => Value;
}