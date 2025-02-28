using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEventsPagination
{
    public class GetEventPaginationQuery : IRequest<IEnumerable<EventResponse>>
    {
        public int Page { get; set; }

        public int Size { get; set; }
    }
}
