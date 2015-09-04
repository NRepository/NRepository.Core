namespace NRepository.Core.Query
{
    using NRepository.Core.Query.Specification;
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Not Thread safe
    /// </summary>
    public class AggregateQueryStrategy<T> : QueryStrategy, IDisposable where T : class
    {
        private AggregateQueryStrategy _InternalAggregateStrategy;
        private bool _Disposed;

        protected AggregateQueryStrategy()
        {
            _InternalAggregateStrategy = new AggregateQueryStrategy();
        }

        public AggregateQueryStrategy(params Expression<Func<T, bool>>[] predicates)
        {
            Check.NotNull(predicates, "predicates");

            var queryStrategies = predicates.Select(predicate => new ExpressionSpecificationQueryStrategy<T>(predicate));
            _InternalAggregateStrategy = new AggregateQueryStrategy(queryStrategies);
        }

        public AggregateQueryStrategy(params IQueryStrategy[] strategies)
        {
            Check.NotNull(strategies, "strategies");

            _InternalAggregateStrategy = new AggregateQueryStrategy(strategies);
        }

        ~AggregateQueryStrategy()
        {
            Dispose(false);
        }

        public IEnumerable<IQueryStrategy> Aggregates
        {
            get { return _InternalAggregateStrategy.Aggregates; }
        }

        public void Add(Expression<Func<T, bool>> predicate)
        {
            _InternalAggregateStrategy.Add(new ExpressionSpecificationQueryStrategy<T>(predicate));
        }

        public void Add(IQueryStrategy queryStrategy)
        {
            _InternalAggregateStrategy.Add(queryStrategy);
        }

        public void AddRange(IEnumerable<IQueryStrategy> queryStrategies)
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            _InternalAggregateStrategy.AddRange(queryStrategies);
        }

        public void AddRange(IEnumerable<Expression<Func<T, bool>>> predicates)
        {
            Check.NotNull(predicates, "queryStrategies");

            _InternalAggregateStrategy.AddRange(predicates.Select(predicate => new ExpressionSpecificationQueryStrategy<T>(predicate)));
        }

        public override IQueryable<T2> GetQueryableEntities<T2>(object additionalQueryData)
        {
            _InternalAggregateStrategy.QueryableRepository = QueryableRepository;
            return _InternalAggregateStrategy.GetQueryableEntities<T2>(additionalQueryData);
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed)
                return;

            _Disposed = true;
            if (disposing)
                _InternalAggregateStrategy.Dispose();
        }
    }
}
