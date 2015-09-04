namespace NRepository.Core.Command
{
    using System;
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public abstract class CommandEventsHandlers : ICommandEventHandlers
    {
       public CommandEventsHandlers()
            : this(new DefaultRepositorySavedHandler())
        {
        }

       public CommandEventsHandlers(IRepositorySubscribe<RepositorySavedEvent> savedEvent)
       {
           Check.NotNull(savedEvent, "savedEvent");
           
           RepositorySavedEventHandler = savedEvent;
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
    }
}
