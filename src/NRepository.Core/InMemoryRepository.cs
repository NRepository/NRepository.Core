namespace NRepository.Core
{
    using System.Collections.Generic;
    using NRepository.Core.Command;
    using NRepository.Core.Query;
    using NRepository.Core.Utilities;

    public class InMemoryRepository : RepositoryBase
    {
        public InMemoryRepository()
            : this(new List<object>(), new DefaultRepositoryEventsHandlers(), new DefaultRepositoryInterceptors())
        {
        }

        public InMemoryRepository(ICollection<object> collection)
            : this(collection, new DefaultRepositoryEventsHandlers(), new DefaultRepositoryInterceptors())
        {
        } 

        public InMemoryRepository(ICollection<object> collection, IRepositoryEventsHandlers queryEventHandlers)
            : this(collection, queryEventHandlers, new DefaultRepositoryInterceptors())
        {
        }

        public InMemoryRepository(ICollection<object> collection, IRepositoryInterceptors repositoryInterceptors)
            : this(collection, new DefaultRepositoryEventsHandlers(), repositoryInterceptors)
        {
        }

        public InMemoryRepository(IRepositoryEventsHandlers queryEventHandlers)
            : this(new List<object>(), queryEventHandlers, new DefaultRepositoryInterceptors())
        {
        }

        public InMemoryRepository(IRepositoryInterceptors repositoryInterceptors)
            : this(new List<object>(), new DefaultRepositoryEventsHandlers(), repositoryInterceptors)
        {
        }

        public InMemoryRepository(
         IRepositoryEventsHandlers eventHandlers,
         IRepositoryInterceptors repositoryInterceptors)
            : this(new List<object>(), eventHandlers, repositoryInterceptors)
        {
        }

        public InMemoryRepository(
            ICollection<object> collection,
            IRepositoryEventsHandlers eventHandlers,
            IRepositoryInterceptors repositoryInterceptors)
            : base(
               new InMemoryQueryRepository(collection, eventHandlers, repositoryInterceptors.QueryInterceptor),
               new InMemoryCommandRepository(collection, eventHandlers, repositoryInterceptors))
        {
            Check.NotNull(collection, "collection");
            Check.NotNull(eventHandlers, "eventHandlers");
            Check.NotNull(repositoryInterceptors, "repositoryInterceptors");

           ObjectContext = collection;
        }

        public IEnumerable<object> Items
        {
            get { return (IEnumerable<object>)ObjectContext; }
        }
    }
}
