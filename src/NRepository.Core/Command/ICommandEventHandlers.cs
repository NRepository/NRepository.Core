namespace NRepository.Core.Command
{
    using NRepository.Core.Events;

    public interface ICommandEventHandlers
    {
        IRepositorySubscribe<EntityAddedEvent> EntityAddedEventHandler { get; }

        IRepositorySubscribe<EntityModifiedEvent> EntityModifiedEventHandler { get; }

        IRepositorySubscribe<EntityDeletedEvent> EntityDeletedEventHandler { get; }

        IRepositorySubscribe<RepositorySavedEvent> RepositorySavedEventHandler { get; }
    }
}