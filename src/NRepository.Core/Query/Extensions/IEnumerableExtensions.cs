namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> AddQueryStrategy<T>(this IEnumerable<T> enumerable, params IQueryStrategy[] strategies) where T : class
        {
            Check.NotNull(enumerable, "enumerable");
            Check.NotNull(strategies, "strategies");

            var singleRepository = new SingleQueryRepository<T>(enumerable.AsQueryable());

            var aggregateStrategy = new AggregateQueryStrategy(strategies);
            aggregateStrategy.QueryableRepository = singleRepository;

            return aggregateStrategy.GetQueryableEntities<T>(null).AsEnumerable();
        }
    }
}
