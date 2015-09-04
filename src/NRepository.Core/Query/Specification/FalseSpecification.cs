namespace NRepository.Core.Query.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// False specification.
    /// </summary>
    /// <typeparam name="T">Type of entity this specification applies to.</typeparam>
    public sealed class FalseSpecification<T>
        : SpecificationQueryStrategy<T>
        where T : class
    {
        /// <summary>
        /// Returns the lambda expression that must be satisfied by the objects matching this specification.
        /// </summary>
        /// <returns>The lambda expression.</returns>
        public override Expression<Func<T, bool>> SatisfiedBy(object additionalQueryData)
        {
            const bool Result = false;

            Expression<Func<T, bool>> trueExpression = t => Result;
            return trueExpression;
        }
    }
}
