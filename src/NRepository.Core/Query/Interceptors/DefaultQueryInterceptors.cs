namespace NRepository.Core.Query
{
    public class DefaultQueryInterceptors : IQueryInterceptors
    {
        public DefaultQueryInterceptors()
        {
            QueryInterceptor = new DefaultQueryInterceptor();
        }

        public IQueryInterceptor QueryInterceptor
        {
            get;
            private set;
        }
    }
}
