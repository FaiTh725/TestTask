using Event.Domain.Common.Specifications;
using LinqKit;

namespace Event.Application.Specifications
{
    public class AndSpecification<TEntity> : Specification<TEntity>
        where TEntity : class
    {
        public AndSpecification(
            Specification<TEntity> left,
            Specification<TEntity> right)
        {
            AddRandeIncludes(left.Includes);
            AddRandeIncludes(right.Includes);

            if (left.Criteria is null)
            {
                Criteria = right.Criteria;
                return;
            }

            if(right.Criteria is null)
            {
                Criteria = left.Criteria;
                return;
            }


            var combineAnd = PredicateBuilder.New<TEntity>(true);
            combineAnd = combineAnd.And(left.Criteria);
            combineAnd = combineAnd.And(right.Criteria);

            Criteria = combineAnd;
        }
    }
}
