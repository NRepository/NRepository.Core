namespace NRepository.Core.Events
{
    using NRepository.Core.Query;
    using NRepository.Core.Utilities;

    public abstract class SimpleRepositoryQueryEvent : RepositoryQueryEvent
    {
        protected SimpleRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy specificationStrategy, IQueryStrategy queryStrategy, object additionalQueryData)
            : this(repository, specificationStrategy, queryStrategy, additionalQueryData, null)
        {
        }

        protected SimpleRepositoryQueryEvent(IQueryRepository repository, IQueryStrategy specificationStrategy, IQueryStrategy queryStrategy, object additionalQueryData, bool? throwExceptionIfZeroOrManyFound)
            : base(repository)
        {
            Check.NotNull(repository, "repository");
            Check.NotNull(specificationStrategy, "specificationStrategy");
            Check.NotNull(queryStrategy, "queryStrategy");

            AdditionalQueryData = additionalQueryData;
            SpecificationStrategy = specificationStrategy;
            QueryStrategy = queryStrategy;
            ThrowExceptionIfZeroOrManyFound = throwExceptionIfZeroOrManyFound;
        }

        public IQueryStrategy SpecificationStrategy
        {
            get;
            private set;
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
