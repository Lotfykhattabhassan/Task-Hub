namespace TaskHub.BuildingBlocks.Application.MultiTenancy;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class SkipTenantResolutionAttribute : Attribute
{
}
