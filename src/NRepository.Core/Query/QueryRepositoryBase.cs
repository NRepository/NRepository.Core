namespace NRepository.Core.Query
{
    using NRepository.Core.Events;
    using NRepository.Core.Query.Specification;
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class QueryRepositoryBase : IQueryRepository, IDisposable
    {
        private bool disposed;

        protected QueryRepositoryBase()
            : this(new DefaultQueryEventHandlers(), new DefaultQueryInterceptor())
        {
        }

        protected QueryRepositoryBase(IQueryEventHandler queryEventHandlers)
            : this(queryEventHandlers, new DefaultQueryInterceptor())
        {
        }

        protected QueryRepositoryBase(IQueryInterceptor queryInterceptor)
            : this(new DefaultQueryEventHandlers(), queryInterceptor)
        {
        }

        protected QueryRepositoryBase(IQueryEventHandler queryEventHandlers, IQueryInterceptor queryInterceptor)
        {
            Check.NotNull(queryEventHandlers, "queryEventHandlers");
            Check.NotNull(queryInterceptor, "queryInterceptor");

            QueryEventHandler = queryEventHandlers;
            QueryInterceptor = queryInterceptor;
        }

        ~QueryRepositoryBase()
        {
            Dispose(false);
        }

        public object ObjectContext
        {
            get;
            protected set;
        }

        protected IQueryInterceptor QueryInterceptor
        {
            get;
            set;
        }

        protected IQueryEventHandler QueryEventHandler
        {
            get;
            set;
        }

        public abstract IQueryable<T> GetQueryableEntities<T>(object additionalQueryData) where T : class;

        public virtual T GetEntity<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound, object additionalQueryData = null) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            queryStrategy.QueryableRepository = this;

            var allResults = queryStrategy.GetQueryableEntities<T>(additionalQueryData).Take(2).AsEnumerable();

            QueryEventHandler.RepositoryQueriedEventHandler.Handle(new GetEntityRepositoryQueryEvent(
                 this,
                 queryStrategy,
                 additionalQueryData,
                 throwExceptionIfZeroOrManyFound));

            if (allResults.Count() != 1 && throwExceptionIfZeroOrManyFound)
            {
                var rowsFound = allResults.Count();
                throw new EntitySearchRepositoryException(rowsFound, typeof(T).Name, queryStrategy.ToString());
            }

            var result = allResults.FirstOrDefault();
            return result;
        }


        public IQueryable<T> GetEntities<T>(IQueryStrategy queryStrategy, object additionalQueryData = null) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            queryStrategy.QueryableRepository = this;

            QueryEventHandler.RepositoryQueriedEventHandler.Handle(new GetEntitiesRepositoryQueryEvent(
                 this,
                 queryStrategy,
                 additionalQueryData));

            var result = queryStrategy.GetQueryableEntities<T>(additionalQueryData);
            return result;
        }

        public T GetEntity<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return GetEntity<T>(predicates.Select(predicate => new ExpressionSpecificationQueryStrategy<T>(predicate)).ToArray());
        }

        public T GetEntity<T>(params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntity<T>(new AggregateQueryStrategy(queryStrategies), true);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return GetEntity<T>(new ExpressionSpecificationQueryStrategy<T>(predicate), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntity<T>(new ExpressionSpecificationQueryStrategy<T>(predicate), queryStrategy, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            var aggQueryStrategy = new AggregateQueryStrategy(new ExpressionSpecificationQueryStrategy<T>(predicate));
            aggQueryStrategy.AddRange(queryStrategies);

            return GetEntity<T>(aggQueryStrategy, true);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntity<T>(new AggregateQueryStrategy(queryStrategy, queryStrategy2), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntity<T>(new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntity<T>(new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntity<T>(
                new AggregateQueryStrategy(new ExpressionSpecificationQueryStrategy<T>(predicate), queryStrategy, queryStrategy2), 
                throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntity<T>(
                new AggregateQueryStrategy(new ExpressionSpecificationQueryStrategy<T>(predicate), queryStrategy, queryStrategy2, queryStrategy3), 
                throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntity<T>(
                new AggregateQueryStrategy(new ExpressionSpecificationQueryStrategy<T>(predicate), queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4), 
                throwExceptionIfZeroOrManyFound);
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return GetEntities<T>(new DefaultQueryStrategy());
        }

        public IQueryable<T> GetEntities<T>(params IQueryStrategy[] queryStrategy) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntities<T>(new AggregateQueryStrategy(queryStrategy));
        }

        public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return GetEntities<T>(new ExpressionSpecificationQueryStrategy<T>(predicate));
        }

        public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            var aggregateQueryStrategy = new AggregateQueryStrategy(new ExpressionSpecificationQueryStrategy<T>(predicate));
            aggregateQueryStrategy.AddRange(queryStrategies);

            return GetEntities<T>(aggregateQueryStrategy);
        }

        public async Task<T> GetEntityAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return await Task.Run(() => GetEntity<T>(predicates));
        }

        public async Task<T> GetEntityAsync<T>(params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntity<T>(queryStrategies));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return await Task.Run(() => GetEntity<T>(predicate, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntity<T>(predicate, queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntity<T>(predicate, queryStrategies));
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => GetEntity<T>(queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => GetEntity<T>(queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => GetEntity<T>(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => GetEntity<T>(predicate, queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => GetEntity<T>(predicate, queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => GetEntity<T>(predicate, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound));
        }

        public async Task<List<T>> GetEntitiesAsync<T>() where T : class
        {
            return await Task.Run(() => GetEntities<T>().ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(params IQueryStrategy[] queryStrategy) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntities<T>(queryStrategy).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return await Task.Run(() => GetEntities<T>(predicate).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntities<T>(predicate, queryStrategies).ToList());
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntity<T>(queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public IQueryable<T> GetEntities<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return GetEntities<T>(new AggregateQueryStrategy(predicates.Select(predicate => new ExpressionSpecificationQueryStrategy<T>(predicate)).ToArray()));
        }

        public async Task<List<T>> GetEntitiesAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return await Task.Run(() => GetEntities<T>(predicates).ToList());
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound, object additionalQueryData = null) where T : class
        {
            return await Task.Run(() => GetEntity<T>(queryStrategy, throwExceptionIfZeroOrManyFound, additionalQueryData));
        }


        public async Task<List<T>> GetEntitiesAsync<T>(IQueryStrategy queryStrategy, object additionalQueryData = null) where T : class
        {
            return await Task.Run(() => GetEntities<T>(queryStrategy, additionalQueryData).ToList());
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
            if (disposed)
                return;

            disposed = true;
            if (disposing)
            {
                if (ObjectContext != null && ObjectContext is IDisposable)
                {
                    ((IDisposable)ObjectContext).Dispose();
                }

                ObjectContext = null;
            }
        }

        public virtual void RaiseEvent<T>(T evnt) where T : class, IRepositoryQueryEvent
        {
            var addedEvent = evnt as RepositoryQueryEvent;
            QueryEventHandler.RepositoryQueriedEventHandler.Handle(addedEvent);
        }

    }
}
