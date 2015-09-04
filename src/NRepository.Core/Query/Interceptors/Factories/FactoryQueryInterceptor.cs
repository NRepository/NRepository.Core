namespace NRepository.Core.Query.Interceptors.Factories
{
    using System.Collections.Generic;
    using System.Linq;

    public class FactoryQueryInterceptor : IQueryInterceptor
    {
        public FactoryQueryInterceptor(params IFactoryQuery[] queryFactories)
        {
            QueryFactories = queryFactories;
        }

        public FactoryQueryInterceptor(IEnumerable<IFactoryQuery> queryFactories)
        {
            QueryFactories = queryFactories;
        }

        public IEnumerable<IFactoryQuery> QueryFactories
        {
            get;
            private set;
        }

        public IQueryable<T> Query<T>(
                    IQueryRepository repository,
                    IQueryable<T> query,
                    object additionalQueryData) where T : class
        {
            var factory = additionalQueryData == null
                    ? QueryFactories.FirstOrDefault(p => p.Type.IsAssignableFrom(typeof(T)))
                    : QueryFactories.FirstOrDefault(p => p.Type.IsAssignableFrom(typeof(T)) && p != additionalQueryData);

            if (factory != null && !factory.IsProcessing)
            {
                if (!factory.IsReentrent)
                {
                    var projectedQuery2 = factory.Query<T>(repository, additionalQueryData);
                    return (IQueryable<T>)projectedQuery2;
                }

                lock (factory)
                {
                    if (!factory.IsProcessing)
                    {
                        factory.IsProcessing = true;
                        var projectedQuery = factory.Query<T>(repository, additionalQueryData);
                        factory.IsProcessing = false;
                        return (IQueryable<T>)projectedQuery;
                    }
                }
            }

            return query;
        }
    }
}
