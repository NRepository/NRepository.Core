namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public sealed class ExpressionQueryStrategy<TEntity> : QueryStrategy
    {
        public ExpressionQueryStrategy(Expression<Func<TEntity, bool>> whereExpression)
            : this(null, whereExpression)
        {
        }

        public ExpressionQueryStrategy(string identifier, Expression<Func<TEntity, bool>> predicate)
        {
            Check.NotNull(predicate, "predicate");

            Identifier = identifier;
            WhereExpression = predicate;
        }

        public Expression<Func<TEntity, bool>> WhereExpression
        {
            get;

        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            var query = QueryableRepository.GetQueryableEntities<T>(additionalQueryData).OfType<TEntity>().Where(WhereExpression);
            return (IQueryable<T>)query;
        }
    }

}
