using System.Linq.Expressions;

namespace Event.Domain.Common.Specifications
{
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression oldValue;
        private readonly Expression newValue;

        public ReplaceExpressionVisitor(
            Expression oldValue, 
            Expression newValue)
        {
            oldValue = oldValue;
            newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            return node == oldValue ? newValue : base.Visit(node);
        }
    }
}
