namespace NRepository.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public class AggregateRepositorySavedEventHandler : IRepositorySubscribe<RepositorySavedEvent>
    {
        public IEnumerable<IRepositorySubscribe<RepositorySavedEvent>> Handlers { get; private set; }

        public AggregateRepositorySavedEventHandler(params IRepositorySubscribe<RepositorySavedEvent>[] saveHandlers)
        {
            Check.NotNull(saveHandlers, "saveHandlers");

            Handlers = saveHandlers;
        }

        public AggregateRepositorySavedEventHandler(IEnumerable<IRepositorySubscribe<RepositorySavedEvent>> saveHandlers)
        {
            Check.NotNull(saveHandlers, "saveHandlers");

            Handlers = saveHandlers;
        }

        public void Handle(RepositorySavedEvent repositoryEvent)
        {
            Check.NotNull(repositoryEvent, "repositoryEvent");

            Handlers.ToList().ForEach(p => p.Handle(repositoryEvent));
        }
    }
}