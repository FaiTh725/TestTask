using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEventById
{
    public class GetEventByIdQuery : IRequest<EventResponse>
    {
        public long Id { get; set; }
    }
}
