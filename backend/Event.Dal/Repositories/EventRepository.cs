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

        public async Task<EventEntity> AddEvent(
            EventEntity eventEntity,
            CancellationToken token = default)
        {
            var entity = await context.Events.AddAsync(eventEntity, token);

            return entity.Entity;
        }

        public async Task<EventEntity?> GetEvent(
            long eventId,
            CancellationToken token = default)
        {
            var eventEntity = await context.Events
                .FirstOrDefaultAsync(x => x.Id == eventId, token);

            return eventEntity;
        }

        public async Task<EventEntity?> GetEvent(
            string eventName,
            CancellationToken token = default)
        {
            var eventEntity = await context.Events
                .FirstOrDefaultAsync(x => x.Name == eventName, token);

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

        public async Task<EventEntity?> GetEventWithMembers(
            long eventId,
            CancellationToken token = default)
        {
            var eventEntity = await context.Events
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == eventId, token);

            return eventEntity;
        }

        public async Task<EventEntity?> GetEventWithMembers(
            string eventName,
            CancellationToken token = default)
        {
            var eventEntity = await context.Events
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Name == eventName, token);

            return eventEntity;
        }

        public async Task RemoveEvent(
            long eventId,
            CancellationToken token = default)
        {
            await context.Events
                .Where(x => x.Id == eventId)
                .ExecuteDeleteAsync(token);
        }

        public async Task UpdateEvent(
            long eventId, 
            EventEntity updateEntity,
            CancellationToken token = default)
        {
            await context.Events
                .Where(x => x.Id == eventId)
                .ExecuteUpdateAsync(setter => setter.
                    SetProperty(p => p.Description, updateEntity.Description).
                    SetProperty(p => p.TimeEvent, updateEntity.TimeEvent).
                    SetProperty(p => p.MaxMember, updateEntity.MaxMember).
                    SetProperty(p => p.Location, updateEntity.Location),
                    token);
        }
    }
}
