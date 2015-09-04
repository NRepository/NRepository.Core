namespace NRepository.Core.Query
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class ReverseQueryStrategy : QueryStrategy
    {
        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            Debug.Assert(QueryableRepository != null);

            var query = QueryableRepository.GetQueryableEntities<T>(additionalQueryData).Reverse();
            return query;
        }
    }
}
