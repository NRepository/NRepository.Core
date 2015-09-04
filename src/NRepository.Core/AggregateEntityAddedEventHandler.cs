namespace NRepository.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public class AggregateEntityAddedEventHandler : IRepositorySubscribe<EntityAddedEvent>
    {
        public IEnumerable<IRepositorySubscribe<EntityAddedEvent>> Handlers { get; private set; }

        public AggregateEntityAddedEventHandler(params IRepositorySubscribe<EntityAddedEvent>[] addHandlers)
            :this((IEnumerable<IRepositorySubscribe<EntityAddedEvent>>)addHandlers)
        {
        }

        public AggregateEntityAddedEventHandler(IEnumerable<IRepositorySubscribe<EntityAddedEvent>> addHandlers)
        {
            Check.NotNull(addHandlers, "addHandlers");

            Handlers = addHandlers;
        }

        public void Handle(EntityAddedEvent repositoryEvent)
        {
            Check.NotNull(repositoryEvent, "repositoryEvent");

            Handlers.ToList().ForEach(p => p.Handle(repositoryEvent));
        }
    }
}