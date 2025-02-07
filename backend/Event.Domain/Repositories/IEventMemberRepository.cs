using CSharpFunctionalExtensions;
using Event.Domain.Entities;

namespace Event.Domain.Repositories
{
    public interface IEventMemberRepository
    {
        Task<Result<EventMember>> GetEventMember(long eventMemberId);

        IQueryable<EventMember> GetEventMembers(long eventId);

        Task<Result<EventMember>> AddEventMember(EventMember eventMember);

        Task CancelMemberParticipation(EventMember eventMember);
    }
}
