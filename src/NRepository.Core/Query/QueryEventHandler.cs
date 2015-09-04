namespace NRepository.Core.Query
{
    using NRepository.Core.Events;
    using NRepository.Core.Utilities;

    public class QueryEventHandler : IQueryEventHandler
    {
        public QueryEventHandler()
            : this(new DefaultQueryEventHandler())
        {
        }

        public QueryEventHandler(IRepositorySubscribe<RepositoryQueryEvent> repositoryQueriedEventHandle)
        {
            Check.NotNull(repositoryQueriedEventHandle, "repositoryQueriedEventHandle");

            RepositoryQueriedEventHandler = repositoryQueriedEventHandle;
        }

        public IRepositorySubscribe<RepositoryQueryEvent> RepositoryQueriedEventHandler
        {
            get;
            set;
        }
    }
}
