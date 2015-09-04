namespace NRepository.Core.Query
{
    using System.Linq;

    /// <summary>
    /// Class QueryStrategyBase
    /// </summary>
    public abstract class QueryStrategy : IQueryStrategy
    {
        public string Identifier
        {
            get;
            protected set;
        }

        public IQueryableRepository QueryableRepository
        {
            get;
            set;
        }

        public abstract IQueryable<T> GetQueryableEntities<T>(object additionalQueryData) where T : class;
    }
}