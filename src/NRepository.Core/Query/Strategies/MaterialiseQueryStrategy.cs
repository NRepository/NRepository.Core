namespace NRepository.Core.Query
{
    using System;
    using System.Linq;

    public class MaterialiseQueryStrategy : QueryStrategy
    {
        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            var list = QueryableRepository.GetQueryableEntities<T>(additionalQueryData).ToArray();
            return list.AsQueryable<T>();
        }
    }
}
