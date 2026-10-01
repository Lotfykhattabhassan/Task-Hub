using TaskHub.BuildingBlocks.Domain.Abstractions;
using TaskHub.Modules.Tenancy.Domain.Enums;
using TaskHub.Modules.Tenancy.Domain.ValueObjects;

namespace TaskHub.Modules.Tenancy.Domain.Entities;

public class Tenant : AggregateRoot<Guid>
{
    private readonly List<TenantMembership> _members = new();

    public TenantName Name { get; private set; } = null!;
    public TenantSlug Slug { get; private set; } = null!;

    public TenantStatus Status { get; private set; }

    public TenantStorageMode StorageMode { get; private set; }

    public string? ConnectionString { get; private set; }

    public IReadOnlyCollection<TenantMembership> Members => _members.AsReadOnly();

    private Tenant() { }

    private Tenant(
        TenantName name,
        TenantSlug slug,
        TenantStorageMode storageMode,
        string? connectionString)
        : base(Guid.NewGuid())
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(slug);

        if (!Enum.IsDefined(storageMode))
            throw new ArgumentOutOfRangeException(nameof(storageMode));

        if (storageMode == TenantStorageMode.Dedicated &&
            string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                "Dedicated tenant must have a connection string.",
                nameof(connectionString));
        }

        if (storageMode == TenantStorageMode.Shared &&
            !string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                "Shared tenant cannot have a connection string.",
                nameof(connectionString));
        }

        Name = name;
        Slug = slug;
        StorageMode = storageMode;
        ConnectionString = connectionString;
        Status = TenantStatus.Active;
    }

    public static Tenant Create(
        TenantName name,
        TenantSlug slug,
        TenantStorageMode storageMode,
        string? connectionString = null)
    {
        return new Tenant(
            name,
            slug,
            storageMode,
            connectionString);
    }

    public void Rename(TenantName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (Status == TenantStatus.Suspended)
            throw new InvalidOperationException(
                "Suspended tenant cannot be renamed.");

        Name = name;

        MarkAsUpdated();
    }

    public void ChangeSlug(TenantSlug slug)
    {
        ArgumentNullException.ThrowIfNull(slug);

        if (Status == TenantStatus.Suspended)
            throw new InvalidOperationException(
                "Suspended tenant cannot change its slug.");

        Slug = slug;

        MarkAsUpdated();
    }

    public void Activate()
    {
        if (Status == TenantStatus.Active)
            return;

        Status = TenantStatus.Active;

        MarkAsUpdated();
    }

    public void Deactivate()
    {
        if (Status == TenantStatus.Inactive)
            return;

        Status = TenantStatus.Inactive;

        MarkAsUpdated();
    }

    public void Suspend()
    {
        if (Status != TenantStatus.Active)
            throw new InvalidOperationException(
                "Only active tenant can be suspended.");

        Status = TenantStatus.Suspended;

        MarkAsUpdated();
    }

    public void AddMember(TenantMembership member)
    {
        ArgumentNullException.ThrowIfNull(member);

        var existingMember = _members
            .FirstOrDefault(x => x.UserId == member.UserId);

        if (existingMember is not null)
        {
            throw new InvalidOperationException(
                "User is already a member of this tenant.");
        }

        if (member.TenantId != Id)
        {
            throw new InvalidOperationException(
                "Membership does not belong to this tenant.");
        }

        _members.Add(member);

        MarkAsUpdated();
    }
}