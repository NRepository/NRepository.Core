namespace NRepository.Core.Events
{
    using NRepository.Core.Query;
    using NRepository.Core.Utilities;

    public abstract class RepositoryQueryEvent : RepositoryEvent, IRepositoryQueryEvent
    {
        protected RepositoryQueryEvent(IQueryRepository queryRepository)
        {
            Check.NotNull(queryRepository, "queryRepository");

            QueryRepository = queryRepository;
        }

        public IQueryRepository QueryRepository { get; }
    }
}