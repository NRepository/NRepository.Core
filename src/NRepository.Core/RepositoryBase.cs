namespace NRepository.Core
{
    using NRepository.Core.Command;
    using NRepository.Core.Events;
    using NRepository.Core.Query;
    using NRepository.Core.Query.Specification;
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class RepositoryBase : IRepository, IDisposable, IRepositoryCommandEventHandler, IRepositoryQueryEventHandler
    {
        private bool _disposed;

        protected RepositoryBase()
        {
        }

        protected RepositoryBase(
            IQueryRepository queryRepository,
            ICommandRepository commandRepository)
        {
            Check.NotNull(queryRepository, "queryRepository");
            Check.NotNull(commandRepository, "commandRepository");

            QueryRepository = queryRepository;
            CommandRepository = commandRepository;
        }

        [ExcludeFromCodeCoverage]
        ~RepositoryBase()
        {
            Dispose(false);
        }

        public object ObjectContext
        {
            get;
            protected set;
        }

        public IQueryRepository QueryRepository
        {
            get;
            protected set;
        }

        public ICommandRepository CommandRepository
        {
            get;
            protected set;
        }

        public virtual void Add<T>(T entity) where T : class
        {
            CommandRepository.Add<T>(entity);
        }

        public virtual void Add<T>(T entity, IAddCommandInterceptor addInterceptor) where T : class
        {
            Check.NotNull(addInterceptor, "addInterceptor");

            addInterceptor.Add(this, CommandRepository.Add, entity);
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            CommandRepository.Delete<T>(entity);
        }

        public virtual void Delete<T>(T entity, IDeleteCommandInterceptor deleteStrategy) where T : class
        {
            Check.NotNull(deleteStrategy, "deleteStrategy");

            deleteStrategy.Delete(this, CommandRepository.Delete, entity);
        }

        public virtual void Modify<T>(T entity) where T : class
        {
            Check.NotNull(entity, "entity");

            CommandRepository.Modify<T>(entity);
        }

        public virtual void Modify<T>(T entity, IModifyCommandInterceptor modifyInterceptor) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(modifyInterceptor, "modifyInterceptor");

            modifyInterceptor.Modify(this, CommandRepository.Modify, entity);
        }

        public virtual int Save()
        {
            var retVal = CommandRepository.Save();
            return retVal;
        }

        public virtual int Save(ISaveCommandInterceptor savingStrategy)
        {
            Check.NotNull(savingStrategy, "savingStrategy");

            var retVal = CommandRepository.Save(savingStrategy);
            return retVal;
        }


        public virtual async Task AddAsync<T>(T entity) where T : class
        {
            Check.NotNull(entity, "entity");

            await Task.Run(() => Add(entity));
        }

        public virtual async Task AddAsync<T>(T entity, IAddCommandInterceptor addInterceptor) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(addInterceptor, "addInterceptor");

            await Task.Run(() => Add(entity, addInterceptor));
        }

        public virtual async Task ModifyAsync<T>(T entity) where T : class
        {
            Check.NotNull(entity, "entity");

            await Task.Run(() => Modify(entity));
        }

        public virtual async Task ModifyAsync<T>(T entity, IModifyCommandInterceptor modifyInterceptor) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(modifyInterceptor, "modifyInterceptor");

            await Task.Run(() => Modify(entity, modifyInterceptor));
        }

        public virtual async Task DeleteAsync<T>(T entity) where T : class
        {
            Check.NotNull(entity, "entity");

            await Task.Run(() => Delete(entity));
        }

        public virtual async Task DeleteAsync<T>(T entity, IDeleteCommandInterceptor deleteStrategy) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(deleteStrategy, "deleteStrategy");

            await Task.Run(() => Delete(entity, deleteStrategy));
        }

        public virtual async Task<int> SaveAsync()
        {
            return await Task<int>.Run(() => Save());
        }

        public virtual async Task<int> SaveAsync(ISaveCommandInterceptor savingStrategy)
        {
            Check.NotNull(savingStrategy, "savingStrategy");

            return await Task<int>.Run(() => Save(savingStrategy));
        }
        public IQueryable<T> GetQueryableEntities<T>(object additionalQueryData = null) where T : class
        {
            return QueryRepository.GetQueryableEntities<T>(additionalQueryData);
        }

        public T GetEntity<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return QueryRepository.GetEntity(predicates);
        }

        public T GetEntity<T>(params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            return QueryRepository.GetEntity<T>(queryStrategies);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return QueryRepository.GetEntity<T>(predicate, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");

            return QueryRepository.GetEntity<T>(predicate, queryStrategy, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return QueryRepository.GetEntity<T>(predicate, queryStrategies);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return QueryRepository.GetEntity<T>(queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return QueryRepository.GetEntity<T>(queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return QueryRepository.GetEntity<T>(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return QueryRepository.GetEntity<T>(predicate, queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return QueryRepository.GetEntity<T>(predicate, queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound);
        }

        public T GetEntity<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return QueryRepository.GetEntity<T>(predicate, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound);
        }
        
        public T GetEntity<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return QueryRepository.GetEntity<T>(queryStrategy, throwExceptionIfZeroOrManyFound);
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return QueryRepository.GetEntities<T>();
        }

        public IQueryable<T> GetEntities<T>(params IQueryStrategy[] queryStrategy) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return QueryRepository.GetEntities<T>(queryStrategy);
        }

        public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return QueryRepository.GetEntities<T>(predicate);
        }

        public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return QueryRepository.GetEntities<T>(predicate, queryStrategies);
        }

        public async Task<T> GetEntityAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return await Task.Run(() => QueryRepository.GetEntity<T>(predicates));
        }

        public async Task<T> GetEntityAsync<T>(params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => QueryRepository.GetEntity<T>(queryStrategies));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return await Task.Run(() => QueryRepository.GetEntity<T>(predicate, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => QueryRepository.GetEntity<T>(predicate, queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => QueryRepository.GetEntity<T>(predicate, queryStrategies));
        }
 
        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => QueryRepository.GetEntity<T>(queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => QueryRepository.GetEntity<T>(queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => QueryRepository.GetEntity<T>(queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");

            return await Task.Run(() => QueryRepository.GetEntity<T>(predicate, queryStrategy, queryStrategy2, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");

            return await Task.Run(() => QueryRepository.GetEntity<T>(predicate, queryStrategy, queryStrategy2, queryStrategy3, throwExceptionIfZeroOrManyFound));
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, IQueryStrategy queryStrategy2, IQueryStrategy queryStrategy3, IQueryStrategy queryStrategy4, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategy, "queryStrategy");
            Check.NotNull(queryStrategy2, "queryStrategy2");
            Check.NotNull(queryStrategy3, "queryStrategy3");
            Check.NotNull(queryStrategy4, "queryStrategy4");

            return await Task.Run(() => QueryRepository.GetEntity<T>(predicate, queryStrategy, queryStrategy2, queryStrategy3, queryStrategy4, throwExceptionIfZeroOrManyFound));
        }
        public async Task<List<T>> GetEntitiesAsync<T>() where T : class
        {
            return await Task.Run(() => QueryRepository.GetEntities<T>().ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(params IQueryStrategy[] queryStrategy) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => QueryRepository.GetEntities<T>(queryStrategy).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            Check.NotNull(predicate, "predicate");

            return await Task.Run(() => QueryRepository.GetEntities<T>(predicate).ToList());
        }

        public async Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
        {
            Check.NotNull(predicate, "predicate");
            Check.NotNull(queryStrategies, "queryStrategies");

            return await Task.Run(() => QueryRepository.GetEntities<T>(predicate, queryStrategies).ToList());
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        {
            Check.NotNull(queryStrategy, "queryStrategy");

            return await Task.Run(() => QueryRepository.GetEntity<T>(queryStrategy, throwExceptionIfZeroOrManyFound));
        }

        public IQueryable<T> GetEntities<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return QueryRepository.GetEntities<T>(predicates);
        }
        public async Task<List<T>> GetEntitiesAsync<T>(params Expression<Func<T, bool>>[] predicates) where T : class
        {
            Check.NotNull(predicates, "predicates");

            return await Task.Run(() => QueryRepository.GetEntities<T>(predicates).ToList());
        }

        public T GetEntity<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound, object additionalQueryData = null) where T : class
        {
            return QueryRepository.GetEntity<T>(queryStrategy, throwExceptionIfZeroOrManyFound, additionalQueryData);
        }

        public async Task<T> GetEntityAsync<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound, object additionalQueryData = null) where T : class
        {
            return await Task.Run(() => QueryRepository.GetEntity<T>(queryStrategy, throwExceptionIfZeroOrManyFound, additionalQueryData));
        }

        public IQueryable<T> GetEntities<T>(IQueryStrategy queryStrategy, object additionalQueryData = null) where T : class
        {
            return QueryRepository.GetEntities<T>(queryStrategy, additionalQueryData);
        }

        public async Task<List<T>> GetEntitiesAsync<T>(IQueryStrategy queryStrategy, object additionalQueryData = null) where T : class
        {
            return await Task.Run(() => QueryRepository.GetEntities<T>(queryStrategy, additionalQueryData).ToList());
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
            if (_disposed)
                return;

            _disposed = true;
            if (disposing)
            {
                if (QueryRepository is IDisposable)
                {
                    ((IDisposable)QueryRepository).Dispose();
                }

                if (CommandRepository is IDisposable)
                {
                    ((IDisposable)CommandRepository).Dispose();
                }
            }
        }

        void IRepositoryQueryEventHandler.RaiseEvent<T>(T evnt)
        {
            Check.NotNull(evnt, "evnt");

            QueryRepository.RaiseEvent<T>(evnt);
        }

        void IRepositoryCommandEventHandler.RaiseEvent<T>(T evnt)
        {
            Check.NotNull(evnt, "evnt");

            CommandRepository.RaiseEvent<T>(evnt);
        }
    }
}