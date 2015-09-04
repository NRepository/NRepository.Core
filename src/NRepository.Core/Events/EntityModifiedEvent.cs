namespace NRepository.Core.Events
{
    using NRepository.Core.Command;

    public class EntityModifiedEvent : RepositoryCommandEntityEvent
    {
        public EntityModifiedEvent(ICommandRepository commandRepository, object entity)
            : base(commandRepository, entity)
        {
        }
    }
}
