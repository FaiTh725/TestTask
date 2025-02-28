using Event.Application.Interfaces;
using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEvents
{
    public class GetEventsQuery : IRequest<IEnumerable<EventResponse>>, ICachQuery
    {
        public string Key => "Events";

        public int? ExpirationSecond => 120;
    }
}
