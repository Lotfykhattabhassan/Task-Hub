using TaskHub.BuildingBlocks.Domain.Abstractions;

namespace TaskHub.Modules.Tasks.Domain.Entities;

// ملاحظة: مفيش TenantId هنا. الـ TenantId بيتضاف كـ shadow property
// في TenantDataDbContext وبيتختم تلقائياً وقت الحفظ.
public class TaskItem : AggregateRoot<Guid>
{
    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsCompleted { get; private set; }

    private TaskItem() { }

    private TaskItem(string title, string? description)
        : base(Guid.NewGuid())
    {
        Title = title;
        Description = description;
    }

    public static TaskItem Create(string title, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Task title cannot be empty.", nameof(title));

        return new TaskItem(title.Trim(), description?.Trim());
    }

    public void Complete()
    {
        if (IsCompleted)
            return;

        IsCompleted = true;
        MarkAsUpdated();
    }
}
