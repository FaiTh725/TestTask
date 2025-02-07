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

        public async Task<Result<EventMember>> AddEventMember(EventMember eventMember)
        {
            if(eventMember is null)
            {
                return Result.Failure<EventMember>("Add null value");
            }

            var entity = await context.Members.AddAsync(eventMember);
        
            await context.SaveChangesAsync();

            return Result.Success(entity.Entity);
        }

        public Task CancelMemberParticipation(EventMember eventMember)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<EventMember>> GetEventMember(long eventMemberId)
        {
            var member = await context.Members
                .FirstOrDefaultAsync(x => x.Id == eventMemberId);

            if (member is null)
            {
                return Result.Failure<EventMember>("Not Found");
            }

            return Result.Success(member);
        }

        public IQueryable<EventMember> GetEventMembers(long eventId)
        {
            return context.Members
                .Include(x => x.EventEntity)
                .Where(x => x.EventEntity == null ? 
                    false : x.EventEntity.Id == eventId);
        }
    }
}
