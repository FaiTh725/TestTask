using Event.Domain.Common.Specifications;
using Event.Domain.Entities;

namespace Event.Domain.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<EventEntity> GetEvents();

        IEnumerable<EventEntity> GetEventsWithMembers();

        IEnumerable<EventEntity> GetEventsWithMembers(int page, int size);

        IEnumerable<EventEntity> GetEvents(Specification<EventEntity> specification);

        Task<EventEntity> AddEvent(EventEntity eventEntity, CancellationToken token = default);

        Task<EventEntity?> GetEvent(long eventId, CancellationToken token = default);

        Task<EventEntity?> GetEvent(string eventName, CancellationToken token = default);

        Task<EventEntity?> GetEventWithMembers(long eventId, CancellationToken token = default);

        Task<EventEntity?> GetEventWithMembers(string eventName, CancellationToken token = default);

        Task RemoveEvent(long eventId, CancellationToken token = default);

        Task UpdateEvent(long eventId, EventEntity updateEntity, CancellationToken token = default);

        // TODO: add images to event
    }
}
