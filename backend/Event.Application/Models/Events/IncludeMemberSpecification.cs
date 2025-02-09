using Event.Domain.Common.Specifications;
using Event.Domain.Entities;

namespace Event.Application.Models.Events
{
    public class IncludeMemberSpecification : Specification<EventEntity>
    {
        public IncludeMemberSpecification() 
        {
            AddInclude(x => x.Members);
        }
    }
}
