namespace NRepository.Core.Events
{
    public class DefaultRepositorySavedHandler : IRepositorySubscribe<RepositorySavedEvent>
    {
        public void Handle(RepositorySavedEvent savedDetails)
        {
        }
    }
}
