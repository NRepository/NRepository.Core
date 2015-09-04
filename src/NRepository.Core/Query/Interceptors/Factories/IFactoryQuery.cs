namespace NRepository.Core.Query.Interceptors.Factories
{
    using System;
    using System.Linq;

    public interface IFactoryQuery
    {
        bool IsProcessing { get; set; }

        bool IsReentrent { get; }

        Type Type { get; }

        IQueryable<object> Query<T>(IQueryRepository repository, object additionalQueryData) where T : class;
    }
}
