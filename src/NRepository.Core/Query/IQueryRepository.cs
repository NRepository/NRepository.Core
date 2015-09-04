namespace NRepository.Core.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using NRepository.Core.Query.Specification;
    using System.Collections.Generic;
    using NRepository.Core.Events;

    public interface IQueryRepository : IQueryableRepository, IRepositoryContext, IRepositoryQueryEventHandler
    {
        T GetEntity<T>(params Expression<Func<T, bool>>[] predicates) where T : class;
        T GetEntity<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(params IQueryStrategy[] queryStrategies) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;
        T GetEntity<T>(ISpecificationQueryStrategy<T> specification) where T : class;
        T GetEntity<T>(ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class;

        T GetEntity<T>(object additionalQueryData, params Expression<Func<T, bool>>[] predicates) where T : class;
        T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, params IQueryStrategy[] queryStrategies) where T : class;
        T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;
        T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification) where T : class;
        T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        T GetEntity<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class;

        Task<T> GetEntityAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class;
        Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
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
        Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification) where T : class;
        Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class;

        Task<T> GetEntityAsync<T>(object additionalQueryData, params Expression<Func<T, bool>>[] predicates) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, params IQueryStrategy[] queryStrategies) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class;
        Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class;


        IQueryable<T> GetEntities<T>() where T : class;
        IQueryable<T> GetEntities<T>(params Expression<Func<T, bool>>[] predicates) where T : class;
        IQueryable<T> GetEntities<T>(params IQueryStrategy[] queryStrategy) where T : class;
        IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate) where T : class;
        IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;
        IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification) where T : class;
        IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy) where T : class;
        IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2) where T : class;
        IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3) where T : class;
        IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4) where T : class;

        IQueryable<T> GetEntities<T>(object additionalQueryData) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, params Expression<Func<T, bool>>[] predicates) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, params IQueryStrategy[] queryStrategies) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, Expression<Func<T, bool>> predicate) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4) where T : class;
        IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class;

        Task<List<T>> GetEntitiesAsync<T>() where T : class;
        Task<List<T>> GetEntitiesAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(params IQueryStrategy[] queryStrategy) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, params Expression<Func<T, bool>>[] predicates) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, params IQueryStrategy[] queryStrategy) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4) where T : class;
        Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies) where T : class;
    }
}
