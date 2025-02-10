using CSharpFunctionalExtensions;
using Event.Application.Specifications;
using Event.Domain.Common.Specifications;
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

        public async Task<Result<EventEntity>> AddEvent(EventEntity eventEntity)
        {
            if(eventEntity is null)
            {
                return Result.Failure<EventEntity>("Event is null");
            }

            var entity = await context.Events.AddAsync(eventEntity);

            await context.SaveChangesAsync();

            return Result.Success(entity.Entity);
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

        public async Task<Result<EventEntity>> GetEvent(string eventName)
        {
            var eventEntity = await context.Events
                .FirstOrDefaultAsync(x => x.Name == eventName);

            if (eventEntity is null)
            {
                return Result.Failure<EventEntity>("Not Found");
            }

            return Result.Success(eventEntity);
        }

        public IQueryable<EventEntity> GetEvents()
        {
            return context.Events;
        }

        public IQueryable<EventEntity> GetEvents(Specification<EventEntity> specification)
        {
            return SpecificationEvaluator
                .GetQuery(context.Events, specification);
        }

        public IQueryable<EventEntity> GetEventsWithMembers()
        {
            return context.Events.
                Include(x => x.Members);
        }

        public async Task<Result<EventEntity>> GetEventWithMembers(long eventId)
        {
            var eventEntity = await context.Events
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == eventId);

            if (eventEntity is null)
            {
                return Result.Failure<EventEntity>("Not Found");
            }

            return Result.Success(eventEntity);
        }

        public async Task<Result<EventEntity>> GetEventWithMembers(string eventName)
        {
            var eventEntity = await context.Events
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Name == eventName);

            if (eventEntity is null)
            {
                return Result.Failure<EventEntity>("Not Found");
            }

            return Result.Success(eventEntity);
        }

        public async Task RemoveEvent(long eventId)
        {
            await context.Events
                .Where(x => x.Id == eventId)
                .ExecuteDeleteAsync();
        }

        public async Task UpdateEvent(long eventId, EventEntity updateEntity)
        {
            await context.Events
                .Where(x => x.Id == eventId)
                .ExecuteUpdateAsync(setter => setter.
                    SetProperty(p => p.Description, updateEntity.Description).
                    SetProperty(p => p.TimeEvent, updateEntity.TimeEvent).
                    SetProperty(p => p.MaxMember, updateEntity.MaxMember).
                    SetProperty(p => p.Location, updateEntity.Location)
                    );
        }
    }
}
