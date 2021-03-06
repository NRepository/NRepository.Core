namespace NRepository.Core.Events
{
    using NRepository.Core.Command;
    using NRepository.Core.Utilities;

    public class RepositorySavedEvent : IRepositoryCommandEvent
    {
        public RepositorySavedEvent(ICommandRepository commandRepository)
        {
            Check.NotNull(commandRepository, "commandRepository");

            CommandRepository = commandRepository;
        }

        public ICommandRepository CommandRepository { get; }
    }
}
