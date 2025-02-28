using Event.Application.Models.Members;
using MediatR;

namespace Event.Application.Queries.EventMember.GetMembersPagination
{
    public class GetMembersPaginationQuery : IRequest<IEnumerable<MemberResponse>>
    {
        public long EventId { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }
    }
}
