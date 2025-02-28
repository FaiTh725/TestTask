using MediatR;

namespace Event.Application.Command.EventMember.CancelPaticipateMember
{
    public class CancelPaticipateMemberCommand : IRequest
    {
        public long MemberId { get; set; }
    }
}
