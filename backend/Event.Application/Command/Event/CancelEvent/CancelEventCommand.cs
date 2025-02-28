using MediatR;

namespace Event.Application.Command.Event.CancelEvent
{
    public class CancelEventCommand : IRequest
    {
        public long EventId { get; set; }
    }
}
