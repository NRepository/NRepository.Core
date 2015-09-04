namespace NRepository.Core.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Query;

    public class FamilyQueryRepository : QueryRepositoryBase
    {
        private List<object> _Objects = PersonsData.Data.Cast<object>().ToList();

        public FamilyQueryRepository()
            : this(new DefaultQueryEventHandlers(), new DefaultQueryInterceptor())
        {
        }

        public FamilyQueryRepository(IQueryInterceptor queryInterceptor)
            : this(new DefaultQueryEventHandlers(), queryInterceptor)
        {
        }

        public FamilyQueryRepository(IQueryEventHandler queryEventHandlers)
            : this(queryEventHandlers, new DefaultQueryInterceptor())
        {
        }

        public FamilyQueryRepository(IQueryEventHandler queryEventHandlers, IQueryInterceptor queryInterceptor)
            : base(queryEventHandlers, queryInterceptor)
        {
            ObjectContext = PersonData;
        }

        public List<object> PersonData
        {
            get { return _Objects; }
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalData)
        {
            var query = PersonData.OfType<T>().AsQueryable<T>();
            var retVal = QueryInterceptor.Query<T>(this, query, additionalData);
            return retVal;
        }
    }
}
