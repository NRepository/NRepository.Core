namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ConditionalQueryStrategy : AggregateQueryStrategy
    {
        public ConditionalQueryStrategy(bool runStrategy, params IQueryStrategy[] queryStrategies)
        {
            Check.NotNull(queryStrategies, "queryStrategiesFunc");

            RunStrategy = runStrategy;
            QueryStrategies = queryStrategies;
        }

        public ConditionalQueryStrategy(bool runStrategy, params Func<IQueryStrategy>[] queryStrategiesFuncs)
        {
            Check.NotNull(queryStrategiesFuncs, "queryStrategies");

            QueryStrategiesFunctions = queryStrategiesFuncs;
            RunStrategy = runStrategy;
        }

        public bool RunStrategy
        {
            get;

        }

        public IEnumerable<Func<IQueryStrategy>> QueryStrategiesFunctions
        {
            get;

        }

        public IEnumerable<IQueryStrategy> QueryStrategies
        {
            get;
            private set;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            if (!RunStrategy)
            {
                var emptyStrategy = new DefaultQueryStrategy();
                emptyStrategy.QueryableRepository = this.QueryableRepository;
                return emptyStrategy.GetQueryableEntities<T>(additionalQueryData);
            }

            if (QueryStrategiesFunctions != null)
                QueryStrategies = QueryStrategiesFunctions.Select(p => p());

            AddRange(QueryStrategies);

            QueryableRepository = this.QueryableRepository;
            return base.GetQueryableEntities<T>(additionalQueryData);
        }
    }

}