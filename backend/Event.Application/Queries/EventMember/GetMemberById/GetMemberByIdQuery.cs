using Event.Application.Interfaces;
using Event.Application.Models.Members;
using MediatR;

namespace Event.Application.Queries.EventMember.GetMemberById
{
    public class GetMemberByIdQuery : IRequest<MemberResponse>, ICachQuery
    {
        public long MemberId { get; set; }

        public string Key => "Members:" + MemberId;

        public int? ExpirationSecond => 60;
    }
}
