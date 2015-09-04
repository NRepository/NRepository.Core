namespace NRepository.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public class AggregateRepositoryQueryEventHandler : IRepositorySubscribe<RepositoryQueryEvent>
    {
        public IEnumerable<IRepositorySubscribe<RepositoryQueryEvent>> Handlers { get; private set; }

        public AggregateRepositoryQueryEventHandler(params IRepositorySubscribe<RepositoryQueryEvent>[] saveHandlers)
        {
            Check.NotNull(saveHandlers, "saveHandlers");
            
            Handlers = saveHandlers;
        }

        public AggregateRepositoryQueryEventHandler(IEnumerable<IRepositorySubscribe<RepositoryQueryEvent>> saveHandlers)
        {
            Check.NotNull(saveHandlers, "saveHandlers");
            
            Handlers = saveHandlers;
        }

        public void Handle(RepositoryQueryEvent repositoryEvent)
        {
            Check.NotNull(repositoryEvent, "repositoryEvent");
            
            Handlers.ToList().ForEach(p => p.Handle(repositoryEvent));
        }
    }
}
