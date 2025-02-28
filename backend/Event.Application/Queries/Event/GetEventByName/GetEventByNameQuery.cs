using Event.Application.Interfaces;
using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEventByName
{
    public class GetEventByNameQuery : IRequest<EventResponse>, ICachQuery
    {
        public string Name { get; set; } = string.Empty;

        public string Key => "Events:" + Name;

        public int? ExpirationSecond => 120;
    }
}
