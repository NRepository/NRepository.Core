namespace NRepository.Core
{
    using NRepository.Core.Events;

    public abstract class RepositoryEventsHandlers : IRepositoryEventsHandlers
    {
        public RepositoryEventsHandlers()
        {
            RepositoryQueriedEventHandler = new DefaultQueryEventHandler();
            RepositorySavedEventHandler = new DefaultRepositorySavedHandler();
            EntityAddedEventHandler = new DefaultEntityAddedHandler();
            EntityModifiedEventHandler = new DefaultEntityModifiedHandler();
            EntityDeletedEventHandler = new DefaultEntityDeletedHandler();
        }

        public IRepositorySubscribe<EntityAddedEvent> EntityAddedEventHandler
        {
            get;
            set;
        }

        public IRepositorySubscribe<EntityModifiedEvent> EntityModifiedEventHandler
        {
            get;
            set;
        }

        public IRepositorySubscribe<EntityDeletedEvent> EntityDeletedEventHandler
        {
            get;
            set;
        }

        public IRepositorySubscribe<RepositorySavedEvent> RepositorySavedEventHandler
        {
            get;
            set;
        }

        public IRepositorySubscribe<RepositoryQueryEvent> RepositoryQueriedEventHandler
        {
            get;
            set;
        }
    }

}
