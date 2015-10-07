namespace NRepository.Core.Query.Specification
{
    using NRepository.Core.Utilities;
    using System;
    using System.Linq.Expressions;

    public sealed class ExpressionSpecificationQueryStrategy<TEntity>
        : SpecificationQueryStrategy<TEntity>
        where TEntity : class
    {
        public ExpressionSpecificationQueryStrategy(Expression<Func<TEntity, bool>> matchingCriteria)
         : this(null, matchingCriteria)
        {
        }

        public ExpressionSpecificationQueryStrategy(string identifier, Expression<Func<TEntity, bool>> matchingCriteria)
        {
            Check.NotNull(matchingCriteria, "matchingCriteria");

            Identifier = identifier;
            MatchingCriteria = matchingCriteria;
        }

        public Expression<Func<TEntity, bool>> MatchingCriteria
        {
            get;
        }

        public override Expression<Func<TEntity, bool>> SatisfiedBy(object additionalQueryData)
        {
            return MatchingCriteria;
        }
    }
}