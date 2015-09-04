namespace NRepository.Core.Events
{
    public interface IRepositorySubscribe<in T> where T : class, IRepositoryEvent
    {
        void Handle(T repositoryEvent);
    }
}