using Event.Domain.Common.Specifications;
using Event.Domain.Entities;

namespace Event.Application.Specifications.Event
{
    public class LocationSpecification : Specification<EventEntity>
    {
        public LocationSpecification(string location)
        {
            Criteria = eventEntity => eventEntity.Location == location;
        }
    }
}
