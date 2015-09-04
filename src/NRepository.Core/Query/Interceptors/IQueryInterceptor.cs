namespace NRepository.Core.Query
{
    using System.Linq;

    public interface IQueryInterceptor
    {
        IQueryable<T> Query<T>(IQueryRepository repository, IQueryable<T> query, object additionalQueryData) where T : class;
    }
}
