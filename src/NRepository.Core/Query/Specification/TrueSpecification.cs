namespace NRepository.Core.Query.Specification
{
    using System;
    using System.Linq.Expressions;

     public sealed class TrueSpecification<T>
        : SpecificationQueryStrategy<T>
        where T : class
    {
        public override Expression<Func<T, bool>> SatisfiedBy(object additionalQueryData)
        {
            const bool Result = true;

            Expression<Func<T, bool>> trueExpression = t => Result;
            return trueExpression;
        }
    }
}
