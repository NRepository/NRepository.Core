namespace NRepository.Core.Events
{
    using NRepository.Core.Query;

    public class GetEntitiesRepositoryQueryEvent : SimpleRepositoryQueryEvent
    {
        public GetEntitiesRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy specificationStrategy, IQueryStrategy queryStrategy, object additionalQueryData)
            : this(repository, specificationStrategy, queryStrategy, additionalQueryData, null)
        {
        }

        public GetEntitiesRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy specificationStrategy, IQueryStrategy queryStrategy, object additionalQueryData, bool? throwExceptionIfZeroOrManyFound)
            : base(repository, specificationStrategy, queryStrategy, additionalQueryData, throwExceptionIfZeroOrManyFound)
        {
        }
    }
}
