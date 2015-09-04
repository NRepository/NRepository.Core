namespace NRepository.Core.Query
{
    using System;
    using System.Linq;
    using NRepository.Core.Utilities;

    public class SwitchQueryStrategy : QueryStrategy
    {
        public SwitchQueryStrategy(Func<QueryStrategy> defaultQueryStrategyFunc, params ConditionalQueryStrategy[] conditionalQueryStrategies)
        {
            Check.NotNull(defaultQueryStrategyFunc, "defaultQueryStrategyFunc");

            ConditionalQueryStrategies = conditionalQueryStrategies;
            DefaultQueryStrategyFunc = defaultQueryStrategyFunc;
        }

        public SwitchQueryStrategy(QueryStrategy defaultQueryStrategy, params ConditionalQueryStrategy[] conditionalQueryStrategies)
        {
            Check.NotNull(defaultQueryStrategy, "defaultQueryStrategy");

            ConditionalQueryStrategies = conditionalQueryStrategies;
            DefaultQueryStrategy = defaultQueryStrategy;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            foreach (var strategy in ConditionalQueryStrategies)
            {
                if (strategy.RunStrategy)
                {
                    strategy.QueryableRepository = this.QueryableRepository;
                    return strategy.GetQueryableEntities<T>(additionalQueryData);
                }
            }

            var defaultStrategy = DefaultQueryStrategyFunc != null
                            ? DefaultQueryStrategyFunc()
                            : DefaultQueryStrategy;

            defaultStrategy.QueryableRepository = this.QueryableRepository;
            return defaultStrategy.GetQueryableEntities<T>(additionalQueryData);
        }

        public Func<QueryStrategy> DefaultQueryStrategyFunc
        {
            get;
            private set;
        }

        public QueryStrategy DefaultQueryStrategy
        {
            get;
            private set;
        }

        public ConditionalQueryStrategy[] ConditionalQueryStrategies
        {
            get;
            private set;
        }
    }
}
