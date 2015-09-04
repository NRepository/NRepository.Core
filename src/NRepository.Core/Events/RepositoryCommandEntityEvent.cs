namespace NRepository.Core.Events
{
    using System;
    using NRepository.Core.Command;
    using NRepository.Core.Utilities;

    public abstract class RepositoryCommandEntityEvent : RepositoryCommandEvent
    {
        protected RepositoryCommandEntityEvent(ICommandRepository commandRepository, object entity)
            : base(commandRepository)
        {
            Check.NotNull(entity, "entity");

            Entity = entity;
        }

        public object Entity
        {
            get;
            private set;
        }
    }
}
