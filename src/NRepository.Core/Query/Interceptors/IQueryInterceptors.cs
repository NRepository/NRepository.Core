namespace NRepository.Core.Query
{
    public interface IQueryInterceptors
    {
        IQueryInterceptor QueryInterceptor { get; }
    }
}
