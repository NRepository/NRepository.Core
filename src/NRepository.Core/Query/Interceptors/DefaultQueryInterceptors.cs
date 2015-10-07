namespace NRepository.Core.Query
{
    public sealed class DefaultQueryInterceptors : IQueryInterceptors
    {
        public DefaultQueryInterceptors()
        {
            QueryInterceptor = new DefaultQueryInterceptor();
        }

        public IQueryInterceptor QueryInterceptor { get; }
    }
}
