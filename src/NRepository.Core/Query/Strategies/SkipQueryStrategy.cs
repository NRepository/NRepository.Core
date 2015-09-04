namespace NRepository.Core.Query
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class SkipQueryStrategy : QueryStrategy
    {
        public SkipQueryStrategy(int skip)
        {
            if (skip < 1)
                throw new ArgumentException("skip cannot be less than 1", "skip");

            Skip = skip;
        }

        public int Skip
        {
            get;
            private set;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            Debug.Assert(QueryableRepository != null);

            var query = this.QueryableRepository.GetQueryableEntities<T>(additionalQueryData).Skip(Skip);
            return query;
        }
    }
}
