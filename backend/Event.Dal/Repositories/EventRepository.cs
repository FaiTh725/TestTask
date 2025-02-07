using CSharpFunctionalExtensions;
using Event.Domain.Entities;
using Event.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Event.Dal.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext context;

        public EventRepository(AppDbContext context)
        {
            this.context = context;    
        }
        public Task<Result<EventEntity>> AddEvent(EventEntity eventEntity)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<EventEntity>> GetEvent(long eventId)
        {
            var eventEntity = await context.Events
                .FirstOrDefaultAsync(x => x.Id == eventId);

            if(eventEntity is null)
            {
                return Result.Failure<EventEntity>("Not Found");
            }

            return Result.Success(eventEntity);
        }

        public IQueryable<EventEntity> GetEvents()
        {
            return context.Events;
        }

        public async Task<bool> RemoveEvent(long eventId)
        {
            await context.Events
                .Where(x => x.Id == eventId)
                .ExecuteDeleteAsync();

            return await context.SaveChangesAsync() > 0;
        }
    }
}
