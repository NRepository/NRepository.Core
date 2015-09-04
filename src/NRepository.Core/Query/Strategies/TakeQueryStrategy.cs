namespace NRepository.Core.Query
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class TakeQueryStrategy : QueryStrategy
    {
        public TakeQueryStrategy(int take)
        {
            if (take < 1)
                throw new ArgumentException("take cannot be less than 1", "take");

            Take = take;
        }

        public int Take
        {
            get;
            private set;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            Debug.Assert(QueryableRepository != null);

            var query = this.QueryableRepository.GetQueryableEntities<T>(additionalQueryData).Take(Take);
            return query;
        }
    }
}