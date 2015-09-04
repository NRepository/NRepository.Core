using NRepository.Core.Events;
namespace NRepository.Core.Command
{
    using NRepository.Core.Utilities;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    public abstract class CommandRepositoryBase : ICommandRepository, IDisposable
    {
        private bool disposed;

        public CommandRepositoryBase()
            : this(new DefaultCommandEventsHandlers())
        {
        }

        public CommandRepositoryBase(ICommandEventHandlers eventHandlers)
        {
            Check.NotNull(eventHandlers, "eventHandlers");

            EventHandlers = eventHandlers;
        }

        ~CommandRepositoryBase()
        {
            Dispose(false);
        }

        protected ICommandEventHandlers EventHandlers
        {
            get;
            set;
        }

        public object ObjectContext
        {
            get;
            protected set;
        }

        public abstract int Save();

        public abstract void Modify<T>(T entity) where T : class;

        public abstract void Delete<T>(T entity) where T : class;

        public abstract void Add<T>(T entity) where T : class;

        public virtual int Save(ISaveCommandInterceptor savingStrategy)
        {
            return savingStrategy.Save(this, Save);
        }

        public virtual void Add<T>(T entity, IAddCommandInterceptor addInterceptor) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(addInterceptor, "addInterceptor");
            
            addInterceptor.Add(this, Add, entity);
        }

        public virtual void Modify<T>(T entity, IModifyCommandInterceptor modifyInterceptor) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(modifyInterceptor, "modifyInterceptor");
            
            modifyInterceptor.Modify(this, Modify, entity);
        }

        public virtual void Delete<T>(T entity, IDeleteCommandInterceptor deleteStrategy) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(deleteStrategy, "deleteStrategy");
            
            deleteStrategy.Delete(this, Delete, entity);
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

        public virtual void RaiseEvent<T>(T evnt) where T : class, IRepositoryCommandEvent
        {
            Check.NotNull(evnt, "evnt");

            // EventHandlers.EntityAddedEventHandler.GetType() != typeof(DefaultEntityAddedHandler)
            if (typeof(EntityAddedEvent).IsAssignableFrom(typeof(T)))
            {
                var addedEvent = evnt as EntityAddedEvent;
                EventHandlers.EntityAddedEventHandler.Handle(addedEvent); ;
                return;
            }

            if (typeof(EntityModifiedEvent).IsAssignableFrom(typeof(T)))
            {
                var modifiedEvent = evnt as EntityModifiedEvent;
                EventHandlers.EntityModifiedEventHandler.Handle(modifiedEvent); ;
                return;
            }

            if (typeof(EntityDeletedEvent).IsAssignableFrom(typeof(T)))
            {
                var deletedEvent = evnt as EntityDeletedEvent;
                EventHandlers.EntityDeletedEventHandler.Handle(deletedEvent); ;
                return;
            }

            if (typeof(RepositorySavedEvent).IsAssignableFrom(typeof(T)))
            {
                var addedEvent = evnt as RepositorySavedEvent;
                EventHandlers.RepositorySavedEventHandler.Handle(addedEvent); ;
                return;
            }

            throw new InvalidOperationException(string.Format("{0} is an unknown CommandEvent", typeof(T).FullName));
        }
    }
}
