namespace NRepository.Core.Query
{
    using NRepository.Core.Query.Specification;
    using NRepository.Core.Utilities;
    using System;

    public static class ConditionalExtensions
    {
        public static IQueryStrategy OnCondition(this QueryStrategy queryStrategy, bool condition)
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            if (condition)
                return queryStrategy;

            var defaultQueryStrategy = new DefaultQueryStrategy();
            defaultQueryStrategy.QueryableRepository = queryStrategy.QueryableRepository;
            return defaultQueryStrategy;
        }

        public static ISpecificationQueryStrategy<T> OnCondition<T>(this SpecificationQueryStrategy<T> queryStrategy, bool condition) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            if (condition)
                return queryStrategy;

            var defaultQueryStrategy = new DefaultSpecificationQueryStrategy<T>();
            return defaultQueryStrategy;
        }

        public static string OnCondition(this string include, bool condition)
        {
            if (condition)
                return include;
            else
                return null;
        }

        public static IQueryStrategy AsQueryStrategy<T>(this ISpecificationQueryStrategy<T> specification)
        {
            return (IQueryStrategy)specification;
        }
    }
}
