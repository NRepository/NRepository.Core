namespace NRepository.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public class AggregateEntityModifiedEventHandler : IRepositorySubscribe<EntityModifiedEvent>
    {
        public IEnumerable<IRepositorySubscribe<EntityModifiedEvent>> Handlers { get; private set; }

        public AggregateEntityModifiedEventHandler(params IRepositorySubscribe<EntityModifiedEvent>[] modifyHandlers)
        {
            Check.NotNull(modifyHandlers, "modifyHandlers");

            Handlers = modifyHandlers;
        }

        public AggregateEntityModifiedEventHandler(IEnumerable<IRepositorySubscribe<EntityModifiedEvent>> modifyHandlers)
        {
            Check.NotNull(modifyHandlers, "modifyHandlers");

            Handlers = modifyHandlers;
        }

        public void Handle(EntityModifiedEvent repositoryEvent)
        {
            Check.NotNull(repositoryEvent, "repositoryEvent");

            Handlers.ToList().ForEach(p => p.Handle(repositoryEvent));
        }
    }
}
