
namespace TaskHub.BuildingBlocks.Domain.Events
{
    public interface IDomainEvent
    {
        public DateTime OccurredOnUtc { get; }
    }
}
