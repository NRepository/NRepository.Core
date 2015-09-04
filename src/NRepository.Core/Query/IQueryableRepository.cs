namespace NRepository.Core.Query
{
    using System.Linq;

    public interface IQueryableRepository
    {
        IQueryable<T> GetQueryableEntities<T>(object additionalQueryData) where T : class;
    }
}
