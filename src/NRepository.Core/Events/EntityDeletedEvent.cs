namespace NRepository.Core.Events
{
    using NRepository.Core.Command;

    public class EntityDeletedEvent : RepositoryCommandEntityEvent
    {
        public EntityDeletedEvent(ICommandRepository commandRepository, object entity)
            : base(commandRepository, entity)
        {
        }
    }
}
