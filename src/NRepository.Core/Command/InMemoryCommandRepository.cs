namespace NRepository.Core.Command
{
    using System;
    using System.Collections.Generic;
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public class InMemoryCommandRepository : CommandRepositoryBase
    {
        public InMemoryCommandRepository()
            : this(new List<object>(), new DefaultCommandEventsHandlers(), new CommandInterceptors())
        {
        }

        public InMemoryCommandRepository(ICommandInterceptors commandInterceptor)
            : this(new List<object>(), new DefaultCommandEventsHandlers(), commandInterceptor)
        {
        }

        public InMemoryCommandRepository(ICommandEventHandlers commandEvents)
            : this(new List<object>(), commandEvents, new CommandInterceptors())
        {
        }

        public InMemoryCommandRepository(ICollection<object> entities)
            : this(entities, new DefaultCommandEventsHandlers(), new CommandInterceptors())
        {
        }

        public InMemoryCommandRepository(ICollection<object> entities, ICommandInterceptors commandInterceptor)
            : this(entities, new DefaultCommandEventsHandlers(), commandInterceptor)
        {
        }

        public InMemoryCommandRepository(ICollection<object> entities, ICommandEventHandlers commandEvents)
            : this(entities, commandEvents, new CommandInterceptors())
        {
        }

        public InMemoryCommandRepository(
            ICollection<object> entities,
            ICommandEventHandlers commandEvents,
            ICommandInterceptors commandInterceptor)
            : base(commandEvents)
        {
            Check.NotNull(entities, "entities");
            Check.NotNull(commandEvents, "commandEvents");
            Check.NotNull(commandInterceptor, "commandInterceptor");

            ObjectContext = Entities = entities;
            CommandInterceptors = commandInterceptor;
        }

        public ICollection<object> Entities
        {
            get;
            private set;
        }

        protected ICommandInterceptors CommandInterceptors
        {
            get;
            set;
        }

        public override void Add<T>(T entity)
        {
            Check.NotNull(entity, "entity");

            CommandInterceptors.AddCommandInterceptor.Add(
                this,
                new Action<T>(p => Entities.Add(p)),
                entity);

            EventHandlers.EntityAddedEventHandler.Handle(new EntityAddedEvent(this, entity));
        }

        public override void Delete<T>(T entity)
        {
            Check.NotNull(entity, "entity");

            CommandInterceptors.DeleteCommandInterceptor.Delete(
              this,
              new Action<T>(p => Entities.Remove(p)),
              entity);

            EventHandlers.EntityDeletedEventHandler.Handle(new EntityDeletedEvent(this, entity));
        }

        public override void Modify<T>(T entity)
        {
            Check.NotNull(entity, "entity");

            CommandInterceptors.ModifyCommandInterceptor.Modify(
               this,
               new Action<T>(p => { }),
               entity);

            EventHandlers.EntityModifiedEventHandler.Handle(new EntityModifiedEvent(this, entity));
        }

        public override int Save()
        {
            CommandInterceptors.SaveCommandInterceptor.Save(
               this,
               new Func<int>(() => { return 0; }));

            EventHandlers.RepositorySavedEventHandler.Handle(new RepositorySavedEvent(this));
            return 0;
        }
    }
}