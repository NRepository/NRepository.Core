namespace NRepository.Core.Command
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public abstract class BatchCommandRepositoryBase : ICommandRepository
    {
        private bool _disposed;
        
        private readonly List<IEntityStateWrapper> _BatchedStorageItems;

        public BatchCommandRepositoryBase()
            : this(new DefaultCommandEventsHandlers())
        {
        }

        public BatchCommandRepositoryBase(ICommandEventHandlers eventHandlers)
        {
            Check.NotNull(eventHandlers, "eventHandlers");

            EventHandlers = eventHandlers;
            _BatchedStorageItems = new List<IEntityStateWrapper>();
        }
  
        [ExcludeFromCodeCoverage]
        ~BatchCommandRepositoryBase()
        {
            Dispose(false);
        }

        public object ObjectContext
        {
            get;
            protected set;
        }

        protected ICommandEventHandlers EventHandlers
        {
            get;
            set;
        }

        public IEnumerable<IEntityStateWrapper> BatchedItems
        {
            get { return _BatchedStorageItems; }
        }

        public virtual void Add<T>(T entity) where T : class
        {
            Check.NotNull(entity, "entity");
            
            Add<T>(entity, new DefaultAddCommandInterceptor());
        }

        public virtual void Add<T>(T entity, IAddCommandInterceptor addInterceptor) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(addInterceptor, "addInterceptor");
            
            _BatchedStorageItems.Add(new EntityStateWrapper
            {
                Entity = entity,
                State = State.Add,
                CommandInterceptor = addInterceptor
            });

            EventHandlers.EntityAddedEventHandler.Handle(new EntityAddedEvent(this, entity));
        }

        public virtual void Modify<T>(T entity) where T : class
        {
            Check.NotNull(entity, "entity");

            Modify<T>(entity, new DefaultModifyCommandInterceptor());
        }

        public virtual void Modify<T>(T entity, IModifyCommandInterceptor modifyInterceptor) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(modifyInterceptor, "modifyInterceptor");

            _BatchedStorageItems.Add(new EntityStateWrapper
             {
                 Entity = entity,
                 State = State.Modify,
                 CommandInterceptor = modifyInterceptor
             });

            EventHandlers.EntityModifiedEventHandler.Handle(new EntityModifiedEvent(this, entity));
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            Check.NotNull(entity, "entity");

            Delete(entity, new DefaultDeleteCommandInterceptor());
        }

        public virtual void Delete<T>(T entity, IDeleteCommandInterceptor removeInterceptor) where T : class
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(removeInterceptor, "removeInterceptor");

            _BatchedStorageItems.Add(new EntityStateWrapper
            {
                Entity = entity,
                State = State.Delete,
                CommandInterceptor = removeInterceptor
            });

            EventHandlers.EntityDeletedEventHandler.Handle(new EntityDeletedEvent(this, entity));
        }

        public virtual int Save(ISaveCommandInterceptor savingStrategy)
        {
            Check.NotNull(savingStrategy, "savingStrategy");

            var retVal = savingStrategy.Save(this, Save);
            return retVal;
        }

        public virtual int Save()
        {
            _BatchedStorageItems.ForEach(p =>
            {
                switch (p.State)
                {
                    case State.Add:
                        AddEntityActioned(p.Entity, (IAddCommandInterceptor)p.CommandInterceptor);
                        break;
                    case State.Modify:
                        ModifyEntityActioned(p.Entity, (IModifyCommandInterceptor)p.CommandInterceptor);
                        break;
                    case State.Delete:
                        DeleteEntityActioned(p.Entity, (IDeleteCommandInterceptor)p.CommandInterceptor);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            var retVal = _BatchedStorageItems.Count;
            _BatchedStorageItems.Clear();
            EventHandlers.RepositorySavedEventHandler.Handle(new RepositorySavedEvent(this));
            return retVal;
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
                if (ObjectContext is IDisposable)
                {
                    ((IDisposable)ObjectContext).Dispose();
                }
            }
        }

        protected abstract void AddEntityActioned<T>(T entity, IAddCommandInterceptor addCommandInterceptor) where T : class;

        protected abstract void ModifyEntityActioned<T>(T entity, IModifyCommandInterceptor modifyCommandInterceptor) where T : class;

        protected abstract void DeleteEntityActioned<T>(T entity, IDeleteCommandInterceptor deleteCommandInterceptor) where T : class;

        public virtual async Task AddAsync<T>(T entity) where T : class
        {
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
            Check.NotNull(deleteStrategy, "deleteStrategy");

            await Task.Run(() => Delete(entity, deleteStrategy));
        }

        public virtual async Task<int> SaveAsync()
        {
            return await SaveAsync(new DefaultSaveCommandInterceptor());
        }

        public virtual async Task<int> SaveAsync(ISaveCommandInterceptor savingStrategy)
        {
            Check.NotNull(savingStrategy, "savingStrategy");

            return await Task<int>.Run(() => Save(savingStrategy));
        }

        public virtual void RaiseEvent<T>(T evnt) where T : class, IRepositoryCommandEvent
        {
            Check.NotNull(evnt, "evnt");

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