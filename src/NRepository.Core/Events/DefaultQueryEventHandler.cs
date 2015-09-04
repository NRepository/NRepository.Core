namespace NRepository.Core.Events
{
    public class DefaultQueryEventHandler : IRepositorySubscribe<RepositoryQueryEvent>
    {
        public void Handle(RepositoryQueryEvent queryDetails)
        {
        }
    }
}
