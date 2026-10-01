using System.Text.RegularExpressions;

namespace TaskHub.Modules.Tenancy.Domain.ValueObjects;

public sealed record TenantSlug
{
    public string Value { get; }

    private TenantSlug(string value)
    {
        Value = value;
    }

    public static TenantSlug Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Tenant slug cannot be empty.",
                nameof(value));

        value = value.Trim().ToLowerInvariant();

        if (value.Length < 2)
            throw new ArgumentException(
                "Tenant slug must be at least 2 characters.",
                nameof(value));

        if (value.Length > 50)
            throw new ArgumentException(
                "Tenant slug cannot exceed 50 characters.",
                nameof(value));

        if (!Regex.IsMatch(value, "^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            throw new ArgumentException(
                "Tenant slug can only contain lowercase letters, numbers and hyphens.",
                nameof(value));

        return new TenantSlug(value);
    }

    public override string ToString() => Value;
}