namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Linq;

    public class DefaultQueryInterceptor : IQueryInterceptor
    {
        public IQueryable<T> Query<T>(IQueryRepository repository, IQueryable<T> query, object additionalQueryData) where T : class
        {
            Check.NotNull(repository, "repository");
            Check.NotNull(query, "query");
       
            return query;
        }
    }
}