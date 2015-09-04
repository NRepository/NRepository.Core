namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    public class OrderByQueryStrategy : QueryStrategy
    {
        public OrderByQueryStrategy(params string[] propertyNames)
        {
            Check.NotEmpty(propertyNames, "propertyNames");

            PropertyNames = propertyNames;
        }

        public IEnumerable<string> PropertyNames
        {
            get;
            private set;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            Debug.Assert(QueryableRepository != null);

            var query = default(IOrderedQueryable<T>);
            foreach (var propName in PropertyNames)
            {
                if (query == null)
                    query = OrderBy(QueryableRepository.GetQueryableEntities<T>(additionalQueryData), propName);
                else
                    query = ThenBy(query, propName);
            }

            return query;
        }

        private static IOrderedQueryable<T> OrderBy<T>(IQueryable<T> items, string propertyName)
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetProperty(propertyName).PropertyType;
            var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var expression = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
            return (IOrderedQueryable<T>)items.Provider.CreateQuery<T>(expression);
        }

        private static IOrderedQueryable<T> ThenBy<T>(IQueryable<T> items, string propertyName)
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetProperty(propertyName).PropertyType;
            var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var expression = Expression.Call(typeof(Queryable), "ThenBy", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
            return (IOrderedQueryable<T>)items.Provider.CreateQuery<T>(expression);
        }
    }
}