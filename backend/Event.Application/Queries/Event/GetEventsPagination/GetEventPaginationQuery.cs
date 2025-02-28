using Event.Application.Interfaces;
using Event.Application.Models.Events;
using MediatR;

namespace Event.Application.Queries.Event.GetEventsPagination
{
    public class GetEventPaginationQuery : 
        IRequest<IEnumerable<EventResponse>>, 
        ICachQuery
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string Key => $"Events-{Page}-{Size}";

        public int? ExpirationSecond => 120;
    }
}
