namespace NRepository.Core
{
    using NRepository.Core.Utilities;
    using Query;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public sealed class EntitySearchRepositoryException : RepositoryException
    {
        public EntitySearchRepositoryException(int rowsFound, string entityName, IQueryStrategy queryStrategy)
        {
            Check.NotEmpty(entityName, "entityName");
            Check.NotNull(queryStrategy, "parameters");

            QueryStrategy = queryStrategy;
            RowsFound = rowsFound;
            EntityName = entityName;
        }

        public int RowsFound { get; }

        public string EntityName { get; }

        public IQueryStrategy QueryStrategy { get; }
    }
}
