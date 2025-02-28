using Event.Domain.Entities;

namespace Event.Domain.Repositories
{
    public interface IEventMemberRepository
    {
        Task<EventMember?> GetEventMember(long eventMemberId, CancellationToken token = default);

        IEnumerable<EventMember> GetEventMembers(long eventId);

        IEnumerable<EventMember> GetEventMembers(long eventId, int page, int size);

        Task<EventMember> AddEventMember(EventMember eventMember, CancellationToken token = default);

        Task RemoveEventMember(long memberId, CancellationToken token = default);
    }
}
