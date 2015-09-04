namespace NRepository.Core.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using NRepository.Core.Events;
    using NRepository.Core.Query.Specification;
    using System.Diagnostics.CodeAnalysis;
    using NRepository.Core.Utilities;

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

        public virtual T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");

            queryStrategy.QueryableRepository = this;

            var allResults = queryStrategy.GetQueryableEntities<T>(additionalQueryData).Where(specification.SatisfiedBy(additionalQueryData)).Take(2).AsEnumerable();

            QueryEventHandler.RepositoryQueriedEventHandler.Handle(new GetEntityRepositoryQueryEvent(
                 this,
                 specification,
                 queryStrategy,
                 additionalQueryData,
                 throwExceptionIfZeroOrManyFound));

            if (allResults.Count() != 1 && throwExceptionIfZeroOrManyFound)
            {
                var rowsFound = allResults.Count();
                var specDetails = specification.SpecificationDetails;
                throw new EntitySearchRepositoryException(rowsFound, typeof(T).Name, specDetails);
            }

            var result = allResults.FirstOrDefault();
            return result;
        }

        public virtual IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");

            queryStrategy.QueryableRepository = this;

            QueryEventHandler.RepositoryQueriedEventHandler.Handle(new GetEntitiesRepositoryQueryEvent(
                 this,
                 specification,
                 queryStrategy,
                 additionalQueryData));

            var result = queryStrategy.GetQueryableEntities<T>(additionalQueryData).Where(specification.SatisfiedBy(additionalQueryData));
            return result;
        }

        public T GetEntity<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return GetEntity<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), predicates.Select(predicate => new ExpressionQueryStrategy<T>(predicate)).ToArray());
        }

        public T GetEntity<T>(params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntity<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategies), true);
        }

        public T GetEntity<T>(ISpecificationQueryStrategy<T> specification) where T : class
        {
            Check.NotNull(specification, "specification");

            return GetEntity<T>((object)null, specification, new DefaultQueryStrategy(), true);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return GetEntity<T>((object)null, new ExpressionSpecificationQueryStrategy<T>(predicate), new DefaultQueryStrategy(), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntity<T>((object)null, new ExpressionSpecificationQueryStrategy<T>(predicate), queryStrategy, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntity<T>((object)null, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategies), true);
        }

        public T GetEntity<T>(ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");

            return GetEntity<T>((object)null, specification, new DefaultQueryStrategy(), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntity<T>((object)null, specification, queryStrategy, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntity<T>((object)null, specification, new AggregateQueryStrategy(queryStrategies), true);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntity<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategy, queryStrategy2), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntity<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntity<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntity<T>((object)null, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategy, queryStrategy2), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntity<T>((object)null, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntity<T>((object)null, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntity<T>((object)null, specification, new AggregateQueryStrategy(queryStrategy, queryStrategy2), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntity<T>((object)null, specification, new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntity<T>((object)null, specification, new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntity<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategies), true);
        }

        public T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification) where T : class
        {
            Check.NotNull(specification, "specification");

            return GetEntity<T>(additionalQueryData, specification, new DefaultQueryStrategy(), true);
        }

        public T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return GetEntity<T>(additionalQueryData, new ExpressionSpecificationQueryStrategy<T>(predicate), new DefaultQueryStrategy(), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntity<T>(additionalQueryData, new ExpressionSpecificationQueryStrategy<T>(predicate), queryStrategy, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntity<T>(additionalQueryData, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategies), true);
        }

        public T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");

            return GetEntity<T>(additionalQueryData, specification, new DefaultQueryStrategy(), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntity<T>(additionalQueryData, specification, new AggregateQueryStrategy(queryStrategies), true);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntity<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), queryStrategy, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntity<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), queryStrategy, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return GetEntity<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(predicates.Select(predicate => new ExpressionSpecificationQueryStrategy<T>(predicate)).ToArray()));
        }

        public T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntity<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategy, queryStrategy2), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntity<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntity<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntity<T>(additionalQueryData, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategy, queryStrategy2), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntity<T>(additionalQueryData, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntity<T>(additionalQueryData, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntity<T>(additionalQueryData, specification, new AggregateQueryStrategy(queryStrategy, queryStrategy2), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntity<T>(additionalQueryData, specification, new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3), throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntity<T>(additionalQueryData, specification, new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4), throwExceptionIfZeroOrManyFound);
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return GetEntities<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), new DefaultQueryStrategy());
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData) where T : class
        {
            return GetEntities<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), new DefaultQueryStrategy());
        }

        public IQueryable<T> GetEntities<T>(params IQueryStrategy[] queryStrategy) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntities<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategy));
        }

        public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return GetEntities<T>((object)null, new ExpressionSpecificationQueryStrategy<T>(predicate), new DefaultQueryStrategy());
        }

        public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntities<T>((object)null, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategies));
        }

        public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification) where T : class
        {
            Check.NotNull(specification, "specification");

            return GetEntities<T>((object)null, specification, new DefaultQueryStrategy());
        }

        public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntities<T>((object)null, specification, new AggregateQueryStrategy(queryStrategies));
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntities<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), new AggregateQueryStrategy(queryStrategies));
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, Expression<Func<T, bool>> predicate) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return GetEntities<T>(additionalQueryData, new ExpressionSpecificationQueryStrategy<T>(predicate), new DefaultQueryStrategy());
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntities<T>(additionalQueryData, new ExpressionSpecificationQueryStrategy<T>(predicate), new AggregateQueryStrategy(queryStrategies));
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification) where T : class
        {
            Check.NotNull(specification, "specification");

            return GetEntities<T>(additionalQueryData, specification, new DefaultQueryStrategy());
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategies, "queryStrategies");

            return GetEntities<T>(additionalQueryData, specification, new AggregateQueryStrategy(queryStrategies));
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

        public async Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification) where T : class
        {
            Check.NotNull(specification, "specification");

            return await Task.Run(() => GetEntity<T>(specification));
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

        public async Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");

            return await Task.Run(() => GetEntity<T>(specification, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntity<T>(specification, queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntity<T>(specification, queryStrategies));
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

        public async Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => GetEntity<T>(specification, queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => GetEntity<T>(specification, queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => GetEntity<T>(specification, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, predicate));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, queryStrategies));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification) where T : class
        {
            Check.NotNull(specification, "specification");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, specification));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, predicate, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntityAsync<T>(additionalQueryData, predicate, queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntityAsync<T>(additionalQueryData, predicate, queryStrategies));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, specification, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, specification, queryStrategies));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, specification, queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public async Task<List<T>> GetEntitiesAsync<T>() where T : class
        {
            return await Task.Run(() => GetEntities<T>().ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData) where T : class
        {
            return await Task.Run(() => GetEntities<T>(additionalQueryData).ToList());
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

        public async Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification) where T : class
        {
            Check.NotNull(specification, "specification");

            return await Task.Run(() => GetEntities<T>(specification).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntities<T>(specification, queryStrategies).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, params IQueryStrategy[] queryStrategy) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, queryStrategy).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, predicate).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, predicate, queryStrategies).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification) where T : class
        {
            Check.NotNull(specification, "specification");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, specification).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, queryStrategies).ToList());
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntity<T>((object)null, new DefaultSpecificationQueryStrategy<T>(), queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, new DefaultSpecificationQueryStrategy<T>(), queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, predicates));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, predicate, queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, predicate, queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, predicate, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, specification, queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, specification, queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => GetEntity<T>(additionalQueryData, specification, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound));
        }

        public IQueryable<T> GetEntities<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return GetEntities<T>(
                default(object),
                new DefaultSpecificationQueryStrategy<T>(),
                new AggregateQueryStrategy(predicates.Select(predicate => new ExpressionSpecificationQueryStrategy<T>(predicate)).ToArray()));
        }

        public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");

            return GetEntities<T>(
                default(object),
                specification,
                new AggregateQueryStrategy(queryStrategy));
        }

        public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntities<T>(
                default(object),
                specification,
                new AggregateQueryStrategy(queryStrategy, queryStrategy2));
        }

        public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntities<T>(
                default(object),
                specification,
                new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3));
        }

        public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntities<T>(
                default(object),
                specification,
                new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4));
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return GetEntities<T>(
                additionalQueryData,
                new DefaultSpecificationQueryStrategy<T>(),
                new AggregateQueryStrategy(predicates.Select(predicate => new ExpressionSpecificationQueryStrategy<T>(predicate)).ToArray()));
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return GetEntities<T>(
                additionalQueryData,
                specification,
                new AggregateQueryStrategy(queryStrategy, queryStrategy2));
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return GetEntities<T>(
                additionalQueryData,
                specification,
                new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3));
        }

        public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return GetEntities<T>(
                additionalQueryData,
                specification,
                new AggregateQueryStrategy(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4));
        }

        public async Task<List<T>> GetEntitiesAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return await Task.Run(() => GetEntities<T>(predicates).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, predicates).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, specification, queryStrategy).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, specification, queryStrategy, queryStrategy2).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, specification, queryStrategy, queryStrategy2, queryStrategy3).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => GetEntities<T>(additionalQueryData, specification, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => GetEntities<T>(default(object), specification, queryStrategy).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => GetEntities<T>(default(object), specification, queryStrategy, queryStrategy2).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => GetEntities<T>(default(object), specification, queryStrategy, queryStrategy2, queryStrategy3).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4) where T : class
        {
            Check.NotNull(specification, "specification");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => GetEntities<T>(default(object), specification, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4).ToList());
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
