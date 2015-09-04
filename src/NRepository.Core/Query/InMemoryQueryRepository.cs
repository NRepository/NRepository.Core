namespace NRepository.Core.Query
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class InMemoryQueryRepository : QueryRepositoryBase
    {
        public InMemoryQueryRepository(IEnumerable<object> entities)
            : this(entities, new DefaultQueryEventHandlers(), new DefaultQueryInterceptor())
        {
        }

        public InMemoryQueryRepository(IEnumerable<object> entities, IQueryEventHandler queryEventHandlers)
            : this(entities, queryEventHandlers, new DefaultQueryInterceptor())
        {
        }

        public InMemoryQueryRepository(IEnumerable<object> entities, IQueryInterceptor queryInterceptor)
            : this(entities, new DefaultQueryEventHandlers(), queryInterceptor)
        {
        }

        public InMemoryQueryRepository(IEnumerable<object> entities, IQueryEventHandler queryEventHandlers, IQueryInterceptor queryInterceptor)
            : base(queryEventHandlers, queryInterceptor)
        {
            Check.NotNull(entities, "entities");

            ObjectContext = entities;
        } 

        public IQueryable<object> Entities
        {
            get { return ((IEnumerable<object>)ObjectContext).AsQueryable(); }
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            var query = Entities.OfType<T>().AsQueryable<T>();
            var retVal = QueryInterceptor.Query(this, query, additionalQueryData);
            return retVal;
        }
    }
}
