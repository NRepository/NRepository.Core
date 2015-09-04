namespace NRepository.Core.Events
{
    using NRepository.Core.Query;

    public class GetEntityRepositoryQueryEvent : SimpleRepositoryQueryEvent
    {
        public GetEntityRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy specification, IQueryStrategy queryStrategy, object additionalQueryData)
            : this(repository, specification, queryStrategy, additionalQueryData, null)
        {

        }
        public GetEntityRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy specification, IQueryStrategy queryStrategy, object additionalQueryData, bool? throwExceptionIfZeroOrManyFound)
            : base(repository, specification, queryStrategy, additionalQueryData, throwExceptionIfZeroOrManyFound)
        {
        }
    }
}
