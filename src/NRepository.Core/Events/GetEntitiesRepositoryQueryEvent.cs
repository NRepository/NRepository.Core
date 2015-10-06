namespace NRepository.Core.Events
{
    using NRepository.Core.Query;

    public class GetEntitiesRepositoryQueryEvent : SimpleRepositoryQueryEvent
    {
        public GetEntitiesRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy queryStrategy, object additionalQueryData)
            : this(repository, queryStrategy, additionalQueryData, null)
        {
        }

        public GetEntitiesRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy queryStrategy, object additionalQueryData, bool? throwExceptionIfZeroOrManyFound)
            : base(repository, queryStrategy, additionalQueryData, throwExceptionIfZeroOrManyFound)
        {
        }
    }
}
