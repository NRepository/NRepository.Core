namespace NRepository.Core
{
    using NRepository.Core.Utilities;
    using System.Collections.Generic;
    using System.Linq;

    public class AggregateRepositoryEventsHandlers : RepositoryEventsHandlers
    {
        public AggregateRepositoryEventsHandlers(params IRepositoryEventsHandlers[] handlers)
        {
            Check.NotNull(handlers, "handlers");

           EntityAddedEventHandler = new AggregateEntityAddedEventHandler(handlers.Select(p => p.EntityAddedEventHandler));
           EntityModifiedEventHandler = new AggregateEntityModifiedEventHandler(handlers.Select(p => p.EntityModifiedEventHandler));
           EntityDeletedEventHandler = new AggregateEntityDeletedEventHandler(handlers.Select(p => p.EntityDeletedEventHandler));
           RepositoryQueriedEventHandler = new AggregateRepositoryQueryEventHandler(handlers.Select(p => p.RepositoryQueriedEventHandler));
           RepositorySavedEventHandler = new AggregateRepositorySavedEventHandler(handlers.Select(p => p.RepositorySavedEventHandler));
        }
    }
}
