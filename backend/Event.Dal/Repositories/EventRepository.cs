using CSharpFunctionalExtensions;
using Event.Domain.Entities;
using Event.Domain.Repositories;

namespace Event.Dal.Repositories
{
    public class EventRepository : IEventRepository
    {
        public Task<EventEntity> AddEvent(EventEntity eventEntity)
        {
            throw new NotImplementedException();
        }

        public Task<Result<EventEntity>> GetEvent(long idEvent)
        {
            throw new NotImplementedException();
        }

        public IQueryable<EventEntity> GetEvents()
        {
            throw new NotImplementedException();
        }
    }
}
