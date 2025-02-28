using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEvents
{
    public class GetEventsQuery : IRequest<IEnumerable<EventResponse>>
    {
    }
}
