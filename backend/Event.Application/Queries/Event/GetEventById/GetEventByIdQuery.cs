using Event.Application.Interfaces;
using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEventById
{
    public class GetEventByIdQuery : IRequest<EventResponse>, ICachQuery
    {
        public long Id { get; set; }

        public string Key => "Events:" + Id.ToString();

        public int? ExpirationSecond => 120;
    }
}
