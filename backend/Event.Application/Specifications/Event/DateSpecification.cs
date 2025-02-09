using Event.Domain.Common.Specifications;
using Event.Domain.Entities;

namespace Event.Application.Specifications.Event
{
    public class DateSpecification : Specification<EventEntity>
    {
        public DateSpecification(DateTime eventTime)
        {
            Criteria = eventEntity => eventEntity.TimeEvent == eventTime;
        }
    }
}
