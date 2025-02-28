using CSharpFunctionalExtensions;
using Event.Domain.Entities;
using Event.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Event.Dal.Repositories
{
    public class EventmemberRepository : IEventMemberRepository
    {
        private readonly AppDbContext context;

        public EventmemberRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<EventMember> AddEventMember(EventMember eventMember)
        {
            var entity = await context.Members.AddAsync(eventMember);
        
            return entity.Entity;
        }

        public async Task RemoveEventMember(long memberId)
        {
            await context.Members.
                Where(x => x.Id == memberId)
                .ExecuteDeleteAsync();
        }

        public async Task<EventMember?> GetEventMember(long eventMemberId)
        {
            return  await context.Members
                .FirstOrDefaultAsync(x => x.Id == eventMemberId);
        }

        public IEnumerable<EventMember> GetEventMembers(long eventId)
        {
            return context.Members
                .Include(x => x.EventEntity)
                .Where(x => x.EventEntity == null ? 
                    false : x.EventEntity.Id == eventId)
                .AsEnumerable();
        }

        public IEnumerable<EventMember> GetEventMembers(
            long eventId, 
            int page, 
            int size)
        {
            return context.Members.Include(x => x.EventEntity)
                .Where(x => x.EventEntity == null ?
                    false : x.EventEntity.Id == eventId)
                .Skip((page - 1) * size)
                .Take(size)
                .OrderBy(x => x.Id)
                .AsEnumerable();
        }
    }
}
