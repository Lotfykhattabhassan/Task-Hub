using Microsoft.AspNetCore.DataProtection;
using TaskHub.Modules.Tenancy.Application.Abstractions;

namespace TaskHub.Modules.Tenancy.Infrastructure.Security;

public sealed class ConnectionStringProtector : IConnectionStringProtector
{
    private readonly IDataProtector _protector;

    public ConnectionStringProtector(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("TaskHub.Tenancy.ConnectionString");
    }

    public string Protect(string connectionString)
        => _protector.Protect(connectionString);

    public string Unprotect(string protectedConnectionString)
        => _protector.Unprotect(protectedConnectionString);
}
