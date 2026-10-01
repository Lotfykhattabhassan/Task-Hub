using Microsoft.EntityFrameworkCore;
using TaskHub.BuildingBlocks.Application.MultiTenancy;
using TaskHub.Modules.Tasks.Domain.Entities;

namespace TaskHub.Modules.Tasks.Infrastructure.Persistence;

public class TenantDataDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public TenantDataDbContext(
        DbContextOptions<TenantDataDbContext> options,
        ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // schema منفصل عشان جداول الـ tasks ما تتلخبطش مع جداول الـ Identity والـ Registry
        modelBuilder.HasDefaultSchema("tasks");

        modelBuilder.Entity<TaskItem>(builder =>
        {
            builder.ToTable("TaskItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property<Guid>("TenantId").IsRequired();

            builder.HasIndex("TenantId");

            builder.HasQueryFilter(x =>
                !x.IsDeleted &&
                EF.Property<Guid>(x, "TenantId") == _tenantContext.TenantId);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampTenantId();
        return base.SaveChangesAsync(cancellationToken);
    }

    // أي سجل جديد بيتختم بالـ TenantId الحالي تلقائياً
    private void StampTenantId()
    {
        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Added))
        {
            if (entry.Metadata.FindProperty("TenantId") is not null)
                entry.Property("TenantId").CurrentValue = _tenantContext.TenantId;
        }
    }
}
