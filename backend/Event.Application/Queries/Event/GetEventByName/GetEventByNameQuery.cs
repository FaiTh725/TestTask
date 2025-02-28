using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEventByName
{
    public class GetEventByNameQuery : IRequest<EventResponse>
    {
        public string Name { get; set; } = string.Empty;
    }
}
