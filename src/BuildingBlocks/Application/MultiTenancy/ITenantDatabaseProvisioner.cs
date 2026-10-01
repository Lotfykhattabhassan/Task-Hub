namespace TaskHub.BuildingBlocks.Application.MultiTenancy;

public interface ITenantDatabaseProvisioner
{
    Task ProvisionAsync(
        string connectionString,
        CancellationToken cancellationToken = default);
}
