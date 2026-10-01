namespace TaskHub.BuildingBlocks.Application.MultiTenancy;

public interface ITenantConnectionStringResolver
{
    string Resolve();
}
