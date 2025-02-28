using MediatR;

namespace Event.Application.Command.Event.UpdateEvent
{
    public class UpdateEventCommand : IRequest
    {
        public long EventId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime TimeEvent { get; set; }

        public int MaxMember { get; set; }
    }
}
