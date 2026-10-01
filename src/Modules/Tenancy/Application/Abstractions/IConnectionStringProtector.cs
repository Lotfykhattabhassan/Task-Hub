namespace TaskHub.Modules.Tenancy.Application.Abstractions;

// تشفير الـ connection string قبل ما تتخزن في الـ DB وفك التشفير وقت الاستخدام
public interface IConnectionStringProtector
{
    string Protect(string connectionString);

    string Unprotect(string protectedConnectionString);
}
