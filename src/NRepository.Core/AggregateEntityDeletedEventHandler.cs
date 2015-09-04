namespace NRepository.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public class AggregateEntityDeletedEventHandler : IRepositorySubscribe<EntityDeletedEvent>
    {
        public IEnumerable<IRepositorySubscribe<EntityDeletedEvent>> Handlers { get; private set; }

        public AggregateEntityDeletedEventHandler(params IRepositorySubscribe<EntityDeletedEvent>[] deleteHandlers)
        {
            Check.NotNull(deleteHandlers, "deleteHandlers");
            
            Handlers = deleteHandlers;
        }

        public AggregateEntityDeletedEventHandler(IEnumerable<IRepositorySubscribe<EntityDeletedEvent>> deleteHandlers)
        {
            Check.NotNull(deleteHandlers, "deleteHandlers");

            Handlers = deleteHandlers;
        }

        public void Handle(EntityDeletedEvent repositoryEvent)
        {
            Check.NotNull(repositoryEvent, "repositoryEvent");

            Handlers.ToList().ForEach(p => p.Handle(repositoryEvent));
        }
    }
}
