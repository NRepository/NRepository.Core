namespace NRepository.Core.Query
{
    using NRepository.Core.Query.Specification;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Utilities;

    public class TextSearchSpecificationStrategy<TEntity> : SpecificationQueryStrategy<TEntity> where TEntity : class
    {
        private class SingleTextSearchSpecificationStrategy : SpecificationQueryStrategy<TEntity>
        {
            private Expression<Func<TEntity, bool>> _Expression;

            public SingleTextSearchSpecificationStrategy(Expression<Func<TEntity, object>> propertyName, string searchString, bool isCaseSensitive = false)
                : this(PropertyInfo<TEntity>.GetMemberName(propertyName), searchString, isCaseSensitive)
            {
            }

            public SingleTextSearchSpecificationStrategy(string propertyName, string searchString, bool isCaseSensitive = false)
            {
                Check.NotEmpty(propertyName, "propertyName");

                IsCaseSensitive = isCaseSensitive;
                PropertyName = propertyName;
                SearchString = searchString;

                if (!string.IsNullOrEmpty(searchString))
                    _Expression = CreateSearchExpression(PropertyName, SearchString, IsCaseSensitive);
                else
                    _Expression = p => true;
            }

            public string SearchString { get; }
            public string PropertyName { get; }
            public bool IsCaseSensitive { get; }

            public override Expression<Func<TEntity, bool>> SatisfiedBy(object additionalQueryData)
            {
                return _Expression;
            }

            private static Expression<Func<TEntity, bool>> CreateSearchExpression(string propertyName, string searchString, bool isCaseSensitive)
            {
                var paramExp = Expression.Parameter(typeof(TEntity), "type");
                var propExp = Expression.Property(paramExp, propertyName);
                var methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                if (!isCaseSensitive)
                {
                    var valueExp2 = Expression.Constant(searchString.ToUpper(), typeof(string));
                    var toUpperMethodInfo = typeof(string).GetMethod("ToUpper", new Type[0]);
                    var upperCall = Expression.Call(propExp, toUpperMethodInfo, null);
                    var methCall2 = Expression.Call(upperCall, methodInfo, valueExp2);
                    return Expression.Lambda<Func<TEntity, bool>>(methCall2, paramExp);
                }

                var valueExp = Expression.Constant(searchString, typeof(string));
                var methCall = Expression.Call(propExp, methodInfo, valueExp);
                return Expression.Lambda<Func<TEntity, bool>>(methCall, paramExp);
            }
        }

        public TextSearchSpecificationStrategy(
            string searchString,
            bool isCaseSensitive,
            params Expression<Func<TEntity, object>>[] propertyNames)
            : this(searchString, isCaseSensitive, propertyNames.Select(p => PropertyInfo<TEntity>.GetMemberName(p)).ToArray())
        {
        }

        public TextSearchSpecificationStrategy(
              string searchString,
              params Expression<Func<TEntity, object>>[] propertyNames)
              : this(searchString, false, propertyNames.Select(p => PropertyInfo<TEntity>.GetMemberName(p)).ToArray())
        {
        }

        public TextSearchSpecificationStrategy(
            string searchString,
            params string[] propertyNames)
            : this(searchString, false, propertyNames)
        {
        }

        public TextSearchSpecificationStrategy(
            string searchString,
            bool isCaseSensitive,
            params string[] propertyNames)
        {
            IsCaseSensitive = isCaseSensitive;
            PropertyNames = propertyNames;
            SearchString = searchString;
        }

        public string SearchString
        {
            get;

        }

        public IEnumerable<string> PropertyNames
        {
            get;

        }

        public bool IsCaseSensitive
        {
            get;

        }

        public override Expression<Func<TEntity, bool>> SatisfiedBy(object additionalQueryData)
        {
            Check.NotEmpty(PropertyNames, "Properties");

            var searchSpecifications = default(SpecificationQueryStrategy<TEntity>);
            foreach (var propName in PropertyNames)
            {
                var spec = new SingleTextSearchSpecificationStrategy(propName, SearchString, IsCaseSensitive);
                if (searchSpecifications == null)
                    searchSpecifications = spec;
                else
                    searchSpecifications = searchSpecifications | spec;
            }

            return searchSpecifications.SatisfiedBy(additionalQueryData);
        }
    }
}
