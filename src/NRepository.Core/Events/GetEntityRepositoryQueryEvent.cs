namespace NRepository.Core.Events
{
    using NRepository.Core.Query;

    public class GetEntityRepositoryQueryEvent : SimpleRepositoryQueryEvent
    {
        public GetEntityRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy queryStrategy, object additionalQueryData)
            : this(repository, queryStrategy, additionalQueryData, null)
        {

        }
        public GetEntityRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy queryStrategy, object additionalQueryData, bool? throwExceptionIfZeroOrManyFound)
            : base(repository, queryStrategy, additionalQueryData, throwExceptionIfZeroOrManyFound)
        {
        }
    }
}
