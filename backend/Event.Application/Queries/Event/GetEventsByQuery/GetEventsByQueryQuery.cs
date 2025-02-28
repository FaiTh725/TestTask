using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEventsByQuery
{
    public class GetEventsByQueryQuery : IRequest<IEnumerable<EventResponse>>
    {
        public string? Location { get; set; } 
        
        public string? Category { get; set; } 
        
        public DateTime? EventTime { get; set; }
    }
}
