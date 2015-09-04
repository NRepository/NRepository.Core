namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Not Thread safe
    /// </summary>
    public class AggregateQueryStrategy : QueryStrategy, IDisposable
    {
        private readonly List<IQueryStrategy> _Aggregates = new List<IQueryStrategy>();
        private bool isReentrant = false;
        private bool _Disposed;

        protected AggregateQueryStrategy()
        {
            _Aggregates = new List<IQueryStrategy>();
        }

        public AggregateQueryStrategy(params IQueryStrategy[] aggregates)
        {
            Check.NotNull(aggregates, "aggregates");

            if (aggregates.Any(p => p == null))
                throw new ArgumentException("Null found in parameter list");

            _Aggregates.AddRange(aggregates);
        }

        public AggregateQueryStrategy(IEnumerable<IQueryStrategy> aggregates)
        {
            Check.NotNull(aggregates, "aggregates");

            if (aggregates.Any(p => p == null))
                throw new ArgumentException("Null found in parameter list");

            _Aggregates.AddRange(aggregates);
        }

        ~AggregateQueryStrategy()
        {
            Dispose(false);
        }

        public IEnumerable<IQueryStrategy> Aggregates
        {
            get { return _Aggregates; }
        }

        public void Add(IQueryStrategy queryStrategy)
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            _Aggregates.Add(queryStrategy);
        }

        public void AddRange(IEnumerable<IQueryStrategy> queryStrategies)
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            _Aggregates.AddRange(queryStrategies);
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            if (!_Aggregates.Any())
                return QueryableRepository.GetQueryableEntities<T>(additionalQueryData);

            if (!isReentrant)
            {
                _Aggregates.First().QueryableRepository = this;

                for (int i = 1; i < _Aggregates.Count(); i++)
                    _Aggregates[i].QueryableRepository = _Aggregates[i - 1];

                isReentrant = true;
                return _Aggregates.Last().GetQueryableEntities<T>(additionalQueryData);
            }

            // Reset to allow reuse,
            isReentrant = false;
            return this.QueryableRepository.GetQueryableEntities<T>(additionalQueryData);
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
            {
                _Aggregates.ForEach(p =>
                {
                    if (p is IDisposable)
                        ((IDisposable)p).Dispose();
                });
            }
        }
    }
}
