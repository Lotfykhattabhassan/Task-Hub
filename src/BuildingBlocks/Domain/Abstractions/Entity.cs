
using TaskHub.BuildingBlocks.Domain.Events;

namespace TaskHub.BuildingBlocks.Domain.Abstractions
{
    public abstract class Entity<TId> 
    {
        public TId Id { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsDeleted { get; protected set; }
        public DateTime? DeletedAt { get; private set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents =>
            _domainEvents.AsReadOnly();
        protected Entity()
        {
            CreatedAt = DateTime.UtcNow;
        }
        protected Entity(TId id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
        }
        public void MarkAsDeleted()
        {
            if (IsDeleted)
                return;

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
        public void Restore()
        {
            if (!IsDeleted)
                return;

            IsDeleted = false;
            DeletedAt = null;
        }
        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
