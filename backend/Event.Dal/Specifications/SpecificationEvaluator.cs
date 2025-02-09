using Event.Domain.Common.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Event.Application.Specifications
{
    public class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(
            IQueryable<TEntity> inputQueryable,
            Specification<TEntity> specification)
            where TEntity : class
        {
            IQueryable<TEntity> query = inputQueryable;

            if (specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.Includes is not null)
            {
                query = specification.Includes
                    .Aggregate(query, (current, include) => 
                        current.Include(include));
            }

            return query;
        }
    }
}
