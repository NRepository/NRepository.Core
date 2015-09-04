namespace NRepository.Core.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using NRepository.Core.Utilities;

    public class ThenByDescendingQueryStrategy<T> : QueryStrategy where T : class
    {
        private List<string> _Properties = new List<string>();

        public ThenByDescendingQueryStrategy()
        {
        }

        public ThenByDescendingQueryStrategy(params Expression<Func<T, object>>[] properties)
        {
            Check.NotNull(properties, "properties");
         
            properties.ToList().ForEach(p => _Properties.Add(PropertyInfo<T>.GetMemberName(p)));
        }

        public IEnumerable<string> Properties
        {
            get { return _Properties; }
        }

        public ThenByDescendingQueryStrategy<T> Add(Expression<Func<T, object>> expression)
        {
            Check.NotNull(expression, "expression");

            return Add(expression, true);
        }

        public ThenByDescendingQueryStrategy<T> Add(Expression<Func<T, object>> expression, bool onCondition)
        {
            if (onCondition)
                _Properties.Add(PropertyInfo<T>.GetMemberName(expression));

            return this;
        }

        public ThenByDescendingQueryStrategy<T> Add(string property)
        {
            return Add(property, true);
        }

        public ThenByDescendingQueryStrategy<T> Add(string property, bool onCondition)
        {
            if (onCondition && !string.IsNullOrEmpty(property))
                _Properties.Add(property);

            return this;
        }

        public override IQueryable<TEntity> GetQueryableEntities<TEntity>(object additionalQueryData)
        {
            if (!_Properties.Any())
                return QueryableRepository.GetQueryableEntities<TEntity>(additionalQueryData);

            var query = QueryableRepository.GetQueryableEntities<TEntity>(additionalQueryData);
            for (int i = 1; i < _Properties.Count; i++)
            {
                query = ThenByDescending(query, _Properties[i]);
            }

            return query;
        }

        private static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(IQueryable<TEntity> items, string propertyName)
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetProperty(propertyName).PropertyType;
            var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var expression = Expression.Call(typeof(Queryable), "ThenByDescending", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
            return (IOrderedQueryable<TEntity>)items.Provider.CreateQuery<T>(expression);
        }
    }
}
