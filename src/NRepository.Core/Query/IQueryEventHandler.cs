namespace NRepository.Core.Query
{
    using NRepository.Core.Events;

    public interface IQueryEventHandler
    {
        IRepositorySubscribe<RepositoryQueryEvent> RepositoryQueriedEventHandler { get; }
    }
}
