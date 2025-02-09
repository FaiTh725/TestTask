using CSharpFunctionalExtensions;
using Event.Domain.Common.Specifications;
using Event.Domain.Entities;

namespace Event.Domain.Repositories
{
    public interface IEventRepository
    {
        IQueryable<EventEntity> GetEvents();

        IQueryable<EventEntity> GetEventsWithMembers();

        IQueryable<EventEntity> GetEvents(Specification<EventEntity> specification);

        Task<Result<EventEntity>> AddEvent(EventEntity eventEntity);

        Task<Result<EventEntity>> GetEvent(long eventId);

        Task<Result<EventEntity>> GetEvent(string eventName);

        Task<Result<EventEntity>> GetEventWithMembers(long eventId);

        Task RemoveEvent(long eventId);

        Task UpdateEvent(long eventId, EventEntity updateEntity);

        // TODO: add images to event
    }
}
