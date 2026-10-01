
namespace TaskHub.Modules.Tenancy.Domain.Enums
{
    [Flags]
    public enum TenantMemberRole
    {
        Owner = 1,
        Admin = 2,
        Manager = 4,
        HR = 8,
        Member = 16
    }
}
