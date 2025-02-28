using Event.Domain.Entities;

namespace Event.Domain.Repositories
{
    public interface IEventMemberRepository
    {
        Task<EventMember?> GetEventMember(long eventMemberId);

        IEnumerable<EventMember> GetEventMembers(long eventId);

        IEnumerable<EventMember> GetEventMembers(long eventId, int page, int size);

        Task<EventMember> AddEventMember(EventMember eventMember);

        Task RemoveEventMember(long memberId);
    }
}
