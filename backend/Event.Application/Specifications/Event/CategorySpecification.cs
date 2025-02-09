using Event.Domain.Common.Specifications;
using Event.Domain.Entities;

namespace Event.Application.Specifications.Event
{
    public class CategorySpecification : Specification<EventEntity>
    {
        public CategorySpecification(string category)
        {
            Criteria = eventEntity => eventEntity.Category == category;
        }
    }
}
