namespace NRepository.Core.Query.Interceptors.Factories
{
    using System;
    using System.Linq;

    public abstract class FactoryQuery<TAssignable> : IFactoryQuery
    {
        private Type _Type = typeof(TAssignable);

        public FactoryQuery(bool isReentrent = false)
        {
            IsReentrent = isReentrent;
        }

        public bool IsProcessing { get; set; }
        public Type Type { get { return _Type; } }
        public bool IsReentrent { get; private set; }

        public abstract IQueryable<object> Query<TEntity>(IQueryRepository repository, object additionalQueryData) where TEntity : class;
    }
}