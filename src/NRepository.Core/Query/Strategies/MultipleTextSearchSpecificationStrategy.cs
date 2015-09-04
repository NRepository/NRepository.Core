namespace NRepository.Core.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using NRepository.Core.Query.Specification;
    using System.Collections.Generic;
    using Utilities;

    public class MultipleTextSearchSpecificationStrategy<TEntity> : SpecificationQueryStrategy<TEntity> where TEntity : class
    {
        public MultipleTextSearchSpecificationStrategy(
            string searchString,
            bool isCaseSensitive,
            params Expression<Func<TEntity, object>>[] propertyNames)
            : this(searchString, isCaseSensitive, propertyNames.Select(p => PropertyInfo<TEntity>.GetMemberName(p)).ToArray())
        {
        }

        public MultipleTextSearchSpecificationStrategy(
              string searchString,
              params Expression<Func<TEntity, object>>[] propertyNames)
              : this(searchString, false, propertyNames.Select(p => PropertyInfo<TEntity>.GetMemberName(p)).ToArray())
        {
        }

        public MultipleTextSearchSpecificationStrategy(
            string searchString,
            params string[] propertyNames)
            : this(searchString,false, propertyNames)
        {
        }

        public MultipleTextSearchSpecificationStrategy(
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
            private set;
        }

        public IEnumerable<string> PropertyNames
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
            Check.NotEmpty(PropertyNames, "Properties");

            var searchSpecifications = default(SpecificationQueryStrategy<TEntity>);
            foreach (var propName in PropertyNames)
            {
                var spec = new TextSearchSpecificationStrategy<TEntity>(propName, SearchString, IsCaseSensitive);
                if (searchSpecifications == null)
                    searchSpecifications = spec;
                else
                    searchSpecifications = searchSpecifications | spec;
            }

            return searchSpecifications.SatisfiedBy(additionalQueryData);
        }
    }
}
