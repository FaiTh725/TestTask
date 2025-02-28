using Event.Application.Models.Members;
using MediatR;

namespace Event.Application.Queries.EventMember.GetMembersByEventId
{
    public class GetMembersByEventIdQuery : IRequest<IEnumerable<MemberResponse>>
    {
        public long EventId { get; set; }
    }
}
