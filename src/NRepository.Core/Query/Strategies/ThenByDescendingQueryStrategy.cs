namespace NRepository.Core.Query
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using NRepository.Core.Utilities;

    public class ThenByDescendingQueryStrategy : QueryStrategy
    {
        public ThenByDescendingQueryStrategy(params string[] propertyNames)
        {
            Check.NotNull(propertyNames, "otherPropertyNames");

            PropertyNames = propertyNames;
        }

        public string PropertyName
        {
            get;
            private set;
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
                    query = ThenByDescending(QueryableRepository.GetQueryableEntities<T>(additionalQueryData), propName);
                else
                    query = ThenByDescending(query, propName);
            }

            return query;
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
