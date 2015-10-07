namespace NRepository.Core.Events
{
    public sealed class DefaultRepositorySavedHandler : IRepositorySubscribe<RepositorySavedEvent>
    {
        public void Handle(RepositorySavedEvent savedDetails)
        {
        }
    }
}
