namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Linq;
    using System.Reflection;

    public class OfTypeQueryStrategy : QueryStrategy
    {
        private readonly MethodInfo _GenericMethod;

        public OfTypeQueryStrategy(Type type)
        {
            Check.NotNull(type, "type");

            Type = type;

            var methodInfo = typeof(Queryable).GetMethod("OfType");
            _GenericMethod = methodInfo.MakeGenericMethod(new[] { Type });
        }

        public Type Type
        {
            get;

        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            var query = (IQueryable<T>)_GenericMethod.Invoke(null, 
                new object[] { QueryableRepository.GetQueryableEntities<T>(additionalQueryData) });
            return query;
        }
    }
}
