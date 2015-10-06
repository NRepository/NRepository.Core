namespace NRepository.Core.Events
{
    using NRepository.Core.Query;
    using NRepository.Core.Utilities;

    public abstract class SimpleRepositoryQueryEvent : RepositoryQueryEvent
    {
        protected SimpleRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy queryStrategy, object additionalQueryData)
            : this(repository, queryStrategy, additionalQueryData, null)
        {
        }

        protected SimpleRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy queryStrategy, object additionalQueryData, bool? throwExceptionIfZeroOrManyFound)
            : base(repository)
        {
            Check.NotNull(repository, "repository");
            Check.NotNull(queryStrategy, "queryStrategy");

            AdditionalQueryData = additionalQueryData;
            QueryStrategy = queryStrategy;
            ThrowExceptionIfZeroOrManyFound = throwExceptionIfZeroOrManyFound;
        }

        public object AdditionalQueryData
        {
            get;
            private set;
        }

        public IQueryStrategy QueryStrategy
        {
            get;
            private set;
        }

        public bool? ThrowExceptionIfZeroOrManyFound
        {
            get;
            private set;
        }
    }
}
