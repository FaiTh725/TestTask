using System.Linq.Expressions;

namespace Event.Domain.Common.Specifications
{
    public abstract class Specification<TEntity> 
        where TEntity : class
    {

        public Expression<Func<TEntity, bool>>? Criteria { get; protected set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddRandeIncludes(List<Expression<Func<TEntity, object>>> includesExpression)
        {
            Includes.AddRange(includesExpression);
        }
    }
}
