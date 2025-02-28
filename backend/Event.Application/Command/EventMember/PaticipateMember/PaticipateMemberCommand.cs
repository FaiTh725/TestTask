using MediatR;

namespace Event.Application.Command.EventMember.PaticipateMember
{
    public class PaticipateMemberCommand : IRequest<long>
    {
        public long EventId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string SecondName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }
    }
}
