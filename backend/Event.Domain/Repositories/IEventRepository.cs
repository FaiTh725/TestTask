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

        Task<EventEntity> AddEvent(EventEntity eventEntity);

        Task<EventEntity?> GetEvent(long eventId);

        Task<EventEntity?> GetEvent(string eventName);

        Task<EventEntity?> GetEventWithMembers(long eventId);

        Task<EventEntity?> GetEventWithMembers(string eventName);

        Task RemoveEvent(long eventId);

        Task UpdateEvent(long eventId, EventEntity updateEntity);

        // TODO: add images to event
    }
}
