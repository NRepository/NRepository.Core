namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class IQueryableExtensions
    {
        public static IQueryable<T> AddQueryStrategy<T>(this IQueryable<T> query, params IQueryStrategy[] strategies) where T : class
        {
            Check.NotNull(query, "query");
            Check.NotNull(strategies, "strategies");

            var singleRepository = new SingleQueryRepository<T>(query);

            var aggregateStrategy = new AggregateQueryStrategy(strategies);
            aggregateStrategy.QueryableRepository = singleRepository;

            return (IQueryable<T>)aggregateStrategy.GetQueryableEntities<T>(null);
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> queryable) where T : class
        {
            return await Task.Run(() =>
            {
                return queryable.FirstOrDefault();
            });
        }

        public static async Task<T> FirstAsync<T>(this IQueryable<T> queryable) where T : class
        {
            return await Task.Run(() =>
            {
                try
                {
                    return queryable.First();
                }
                catch (InvalidOperationException)
                {
                    throw new EntitySearchRepositoryException(0, typeof(T).Name, null);
                }
            });
        }

        public static async Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> queryable) where T : class
        {
            return await Task.Run(() =>
            {
                try
                {
                    return queryable.SingleOrDefault();
                }
                catch (InvalidOperationException)
                {
                    throw new EntitySearchRepositoryException(queryable.Count(), typeof(T).Name, null);
                }
            });
        }

        public static async Task<T> SingleAsync<T>(this IQueryable<T> queryable) where T : class
        {
            return await Task.Run(() =>
            {
                try
                {
                    return queryable.Single();
                }
                catch (InvalidOperationException)
                {
                    throw new EntitySearchRepositoryException(queryable.Count(), typeof(T).Name, null);
                }
            });
        }

        public static async Task<IEnumerable<T>> AsAsync<T>(this IQueryable<T> queryable) where T : class
        {
            return await Task.Run(() =>
            {
                return queryable.ToArray();
            });
        }
    }
}
