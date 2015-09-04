namespace NRepository.Core.Query
{
    using System;
    using System.Linq.Expressions;
    using NRepository.Core.Query.Specification;
    using NRepository.Core.Utilities;

    public class TextSearchSpecificationStrategy<TEntity> : SpecificationQueryStrategy<TEntity> where TEntity : class
    {
        private Expression<Func<TEntity, bool>> _Expression;

        public TextSearchSpecificationStrategy(Expression<Func<TEntity, object>> propertyName, string searchString, bool isCaseSensitive = false)
            : this(PropertyInfo<TEntity>.GetMemberName(propertyName), searchString, isCaseSensitive)
        {
        }

        public TextSearchSpecificationStrategy(string propertyName, string searchString, bool isCaseSensitive = false)
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

        public string SearchString
        {
            get;
            private set;
        }

        public string PropertyName
        {
            get;
            private set;
        }

        public bool IsCaseSensitive
        {
            get;
            private set;
        }

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
}
