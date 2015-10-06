namespace NRepository.Core.Query
{
    using NRepository.Core.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IQueryRepository : IQueryableRepository, IRepositoryContext, IRepositoryQueryEventHandler
    {
        T GetEntity<T>(params Expression<Func<T, bool>>[] predicates) where T : class;
        T GetEntity<T>(params IQueryStrategy[] queryStrategies) where T : class;
        T GetEntity<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound, object additionalQueryData = null) where T : class;
        T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;

        Task<T> GetEntityAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class;
        Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound, object additionalQueryData = null) where T : class;
        Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(params IQueryStrategy[] queryStrategies) where T : class;
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;

        IQueryable<T> GetEntities<T>() where T : class;
        IQueryable<T> GetEntities<T>(params Expression<Func<T, bool>>[] predicates) where T : class;
        IQueryable<T> GetEntities<T>(IQueryStrategy queryStrategy, object additionalQueryData = null) where T : class;
        IQueryable<T> GetEntities<T>(params IQueryStrategy[] queryStrategies) where T : class;
        IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;

        Task<List<T>> GetEntitiesAsync<T>() where T : class;
        Task<List<T>> GetEntitiesAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(IQueryStrategy queryStrategy, object additionalQueryData = null) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(params IQueryStrategy[] queryStrategy) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;
    }
}
