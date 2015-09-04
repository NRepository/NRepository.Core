namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class OrderByDescendingQueryStrategy<TEntity> : QueryStrategy where TEntity : class
    {
        private List<string> _Properties = new List<string>();

        public OrderByDescendingQueryStrategy()
        {
        }

        public OrderByDescendingQueryStrategy(params Expression<Func<TEntity, object>>[] properties)
        {
            Check.NotNull(properties, "properties");
            
            properties.ToList().ForEach(p => _Properties.Add(PropertyInfo<TEntity>.GetMemberName(p)));
        }

        public IEnumerable<string> Properties
        {
            get { return _Properties; }
        }

        public OrderByDescendingQueryStrategy<TEntity> Add(Expression<Func<TEntity, object>> func)
        {
            return Add(func, true);
        }

        public OrderByDescendingQueryStrategy<TEntity> Add(Expression<Func<TEntity, object>> func, bool onCondition)
        {
            if (onCondition)
                _Properties.Add(PropertyInfo<TEntity>.GetMemberName(func));

            return this;
        }
        public OrderByDescendingQueryStrategy<TEntity> Add(string property)
        {
            return Add(property, true);
        }

        public OrderByDescendingQueryStrategy<TEntity> Add(string property, bool onCondition)
        {
            if (onCondition && !string.IsNullOrEmpty(property))
                _Properties.Add(property);

            return this;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            if (!_Properties.Any())
                return QueryableRepository.GetQueryableEntities<T>(additionalQueryData);

            var query = OrderByDescending(QueryableRepository.GetQueryableEntities<T>(additionalQueryData), _Properties.First());
            for (int i = 1; i < _Properties.Count; i++)
            {
                query = ThenByDescending(query, _Properties[i]);
            }

            return query;
        }

        private static IOrderedQueryable<T> OrderByDescending<T>(IQueryable<T> items, string propertyName)
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetProperty(propertyName).PropertyType;
            var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var expression = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
            return (IOrderedQueryable<T>)items.Provider.CreateQuery<T>(expression);
        }

        private static IOrderedQueryable<T> ThenByDescending<T>(IQueryable<T> items, string propertyName)
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetProperty(propertyName).PropertyType;
            var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var expression = Expression.Call(typeof(Queryable), "ThenByDescending", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
            return (IOrderedQueryable<T>)items.Provider.CreateQuery<T>(expression);
        }
    }
}
