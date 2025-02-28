using Event.Application.Models.Members;
using MediatR;

namespace Event.Application.Queries.EventMember.GetMemberById
{
    public class GetMemberByIdQuery : IRequest<MemberResponse>
    {
        public long MemberId { get; set; }
    }
}
