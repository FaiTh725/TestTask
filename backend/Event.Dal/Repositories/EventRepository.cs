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

        public async Task<EventEntity> AddEvent(EventEntity eventEntity)
        {
            var entity = await context.Events.AddAsync(eventEntity);

            return entity.Entity;
        }

        public async Task<EventEntity?> GetEvent(long eventId)
        {
            var eventEntity = await context.Events
                .FirstOrDefaultAsync(x => x.Id == eventId);

            return eventEntity;
        }

        public async Task<EventEntity?> GetEvent(string eventName)
        {
            var eventEntity = await context.Events
                .FirstOrDefaultAsync(x => x.Name == eventName);

            return eventEntity;
        }

        public IEnumerable<EventEntity> GetEvents()
        {
            return context.Events.AsEnumerable();
        }

        public IEnumerable<EventEntity> GetEvents(Specification<EventEntity> specification)
        {
            return SpecificationEvaluator
                .GetQuery(context.Events, specification)
                .AsEnumerable();
        }

        public IEnumerable<EventEntity> GetEventsWithMembers()
        {
            return context.Events.
                Include(x => x.Members)
                .AsEnumerable();
        }

        public IEnumerable<EventEntity> GetEventsWithMembers(int page, int size)
        {
            return context.Events
                .Include(x => x.Members)
                .Skip((page - 1) * size)
                .Take(size)
                .OrderBy(x => x.Id)
                .AsEnumerable();
        }

        public async Task<EventEntity?> GetEventWithMembers(long eventId)
        {
            var eventEntity = await context.Events
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == eventId);

            return eventEntity;
        }

        public async Task<EventEntity?> GetEventWithMembers(string eventName)
        {
            var eventEntity = await context.Events
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Name == eventName);

            return eventEntity;
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
