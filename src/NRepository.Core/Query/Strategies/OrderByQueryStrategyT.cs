namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class OrderByQueryStrategy<T> : QueryStrategy where T : class
    {
        private List<string> _Properties = new List<string>();

        public OrderByQueryStrategy()
        {
        }

        public OrderByQueryStrategy(params Expression<Func<T, object>>[] properties)
        {
            Check.NotNull(properties, "properties");

            properties.ToList().ForEach(p => _Properties.Add(PropertyInfo<T>.GetMemberName(p)));
        }

        public IEnumerable<string> Properties
        {
            get { return _Properties; }
        }

        public OrderByQueryStrategy<T> Add(Expression<Func<T, object>> func)
        {
            return Add(func, true);
        }

        public OrderByQueryStrategy<T> Add(Expression<Func<T, object>> func, bool onCondition)
        {
            if (onCondition)
                _Properties.Add(PropertyInfo<T>.GetMemberName(func));

            return this;
        }

        public OrderByQueryStrategy<T> Add(string property)
        {
            return Add(property, true);
        }

        public OrderByQueryStrategy<T> Add(string property, bool onCondition)
        {
            if (onCondition && !string.IsNullOrEmpty(property))
                _Properties.Add(property);

            return this;
        }

        public override IQueryable<TEntity> GetQueryableEntities<TEntity>(object additionalQueryData)
        {
            if (!_Properties.Any())
                return QueryableRepository.GetQueryableEntities<TEntity>(additionalQueryData);

            var query = OrderBy(QueryableRepository.GetQueryableEntities<TEntity>(additionalQueryData), _Properties.First());
            for (int i = 1; i < _Properties.Count; i++)
            {
                query = ThenBy(query, _Properties[i]);
            }

            return query;
        }

        private static IOrderedQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> items, string propertyName)
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetProperty(propertyName).PropertyType;
            var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var expression = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
            return (IOrderedQueryable<TEntity>)items.Provider.CreateQuery<T>(expression);
        }

        private static IOrderedQueryable<TEntity> ThenBy<TEntity>(IQueryable<TEntity> items, string propertyName)
        {
            var typeOfT = typeof(TEntity);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetProperty(propertyName).PropertyType;
            var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var expression = Expression.Call(typeof(Queryable), "ThenBy", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
            return (IOrderedQueryable<TEntity>)items.Provider.CreateQuery<TEntity>(expression);
        }
    }
}
