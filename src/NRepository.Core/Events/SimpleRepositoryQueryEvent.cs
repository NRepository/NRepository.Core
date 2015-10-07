namespace NRepository.Core.Events
{
    using NRepository.Core.Query;
    using NRepository.Core.Utilities;

    public class SimpleRepositoryQueryEvent : RepositoryQueryEvent
    {
        public SimpleRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy queryStrategy, object additionalQueryData)
            : this(repository, queryStrategy, additionalQueryData, null)
        {
        }

        public SimpleRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy queryStrategy, object additionalQueryData, bool? throwExceptionIfZeroOrManyFound)
            : base(repository)
        {
            Check.NotNull(repository, "repository");
            Check.NotNull(queryStrategy, "queryStrategy");

            AdditionalQueryData = additionalQueryData;
            QueryStrategy = queryStrategy;
            ThrowExceptionIfZeroOrManyFound = throwExceptionIfZeroOrManyFound;
        }

        public object AdditionalQueryData { get; }

        public IQueryStrategy QueryStrategy { get; }

        public bool? ThrowExceptionIfZeroOrManyFound { get; }
    }
}
