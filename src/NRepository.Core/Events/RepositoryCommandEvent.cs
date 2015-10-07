namespace NRepository.Core.Events
{
    using NRepository.Core.Command;
    using NRepository.Core.Utilities;

    public abstract class RepositoryCommandEvent : RepositoryEvent, IRepositoryCommandEvent
    {
        protected RepositoryCommandEvent(ICommandRepository commandRepository)
        {
            Check.NotNull(commandRepository, "commandRepository");

            CommandRepository = commandRepository;
        }

        public ICommandRepository CommandRepository { get; }
    }
}
