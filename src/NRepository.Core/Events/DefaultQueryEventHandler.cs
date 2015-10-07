namespace NRepository.Core.Events
{
    public sealed class DefaultQueryEventHandler : IRepositorySubscribe<RepositoryQueryEvent>
    {
        public void Handle(RepositoryQueryEvent queryDetails)
        {
        }
    }
}
