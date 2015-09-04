namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System.Linq;

    public class SingleQueryRepository<T> : IQueryableRepository where T : class
    {
        private readonly IQueryable<T> _Query;

        public SingleQueryRepository(IQueryable<T> query)
        {
            Check.NotNull(query, "query");

            _Query = query;
        }

        public IQueryable<TEntity> GetQueryableEntities<TEntity>(object additionalQueryData) where TEntity : class
        {
            return (IQueryable<TEntity>)_Query;
        }
    }
}
