using CSharpFunctionalExtensions;
using Event.Domain.Entities;

namespace Event.Domain.Repositories
{
    public interface IEventRepository
    {
        IQueryable<EventEntity> GetEvents();

        Task<EventEntity> AddEvent(EventEntity eventEntity);

        Task<Result<EventEntity>> GetEvent(long idEvent);
    }
}
