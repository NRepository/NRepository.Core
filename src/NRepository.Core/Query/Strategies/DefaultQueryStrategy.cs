namespace NRepository.Core.Query
{
    using System;
    using System.Linq;

    public class DefaultQueryStrategy : QueryStrategy
    {
        public static readonly IQueryStrategy Default = new DefaultQueryStrategy();

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            return QueryableRepository.GetQueryableEntities<T>(additionalQueryData);
        }
    }
}