namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ThenByQueryStrategy<T> : QueryStrategy where T : class
    {
        public ThenByQueryStrategy()
        {
            Properties = new List<string>();
        }

        public ThenByQueryStrategy(params Expression<Func<T, object>>[] properties)
        {
            Check.NotNull(properties, "properties");

            Properties = properties.Select(p => PropertyInfo<T>.GetMemberName(p)).ToList();
        }

        public ThenByQueryStrategy(params  string[] properties)
        {
            Check.NotNull(properties, "properties");

            Properties = properties.ToList();
        }

        public List<string> Properties { get; set; }

        public ThenByQueryStrategy<T> Add(Expression<Func<T, object>> expression)
        {
            Check.NotNull(expression, "expression");

            return Add(expression, true);
        }

        public ThenByQueryStrategy<T> Add(Expression<Func<T, object>> func, bool onCondition)
        {
            if (onCondition)
                Properties.Add(PropertyInfo<T>.GetMemberName(func));

            return this;
        }

        public ThenByQueryStrategy<T> Add(string property)
        {
            return Add(property, true);
        }

        public ThenByQueryStrategy<T> Add(string property, bool onCondition)
        {
            if (onCondition && !string.IsNullOrEmpty(property))
                Properties.Add(property);

            return this;
        }

        public override IQueryable<TEntity> GetQueryableEntities<TEntity>(object additionalQueryData)
        {
            if (!Properties.Any())
                return QueryableRepository.GetQueryableEntities<TEntity>(additionalQueryData);

            var query = QueryableRepository.GetQueryableEntities<TEntity>(additionalQueryData);
            for (int i = 0; i < Properties.Count; i++)
            {
                query = ThenBy(query, Properties[i]);
            }

            return query;
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
