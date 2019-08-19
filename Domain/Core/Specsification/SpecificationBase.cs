using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QAP4.Domain.Core.Specification
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        private Func<T, bool> _compiledExpression;
        protected SpecificationBase(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }
        private Func<T, bool> CompiledExpression => _compiledExpression ?? (_compiledExpression = Criteria.Compile());
        public bool IsSatisfiedBy(T entity)
        {
            return CompiledExpression(entity);
        }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}