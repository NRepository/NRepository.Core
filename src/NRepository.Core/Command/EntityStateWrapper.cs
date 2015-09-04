namespace NRepository.Core.Command
{
    using System;
    using System.Collections.Generic;
    using NRepository.Core.Utilities;

    public class EntityStateWrapper : IEntityStateWrapper
    {
        public EntityStateWrapper(State state, object entity, ICommandInterceptor commandInterceptor)
        {
            Check.NotNull(entity, "entity");
            Check.NotNull(commandInterceptor, "commandInterceptor");

            State = state;
            Entity = entity;
            CommandInterceptor = commandInterceptor;
        }

        public EntityStateWrapper()
        {
        }

        public object Entity
        {
            get;
            set;
        }

        public State State
        {
            get;
            set;
        }

        public ICommandInterceptor CommandInterceptor
        {
            get;
            set;
        }
    }
}
