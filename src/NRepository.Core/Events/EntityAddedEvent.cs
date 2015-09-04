namespace NRepository.Core.Events
{
    using NRepository.Core.Command;

    public class EntityAddedEvent : RepositoryCommandEntityEvent
    {
        public EntityAddedEvent(ICommandRepository commandRepository, object entity)
            : base(commandRepository, entity)
        {
        }
    }
}
