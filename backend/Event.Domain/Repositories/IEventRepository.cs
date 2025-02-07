using CSharpFunctionalExtensions;
using Event.Domain.Entities;

namespace Event.Domain.Repositories
{
    public interface IEventRepository
    {
        IQueryable<EventEntity> GetEvents();

        Task<Result<EventEntity>> AddEvent(EventEntity eventEntity);

        Task<Result<EventEntity>> GetEvent(long eventId);

        Task<bool> RemoveEvent(long eventId);

        // TODO get by property

        // TODO add images to event
    }
}
