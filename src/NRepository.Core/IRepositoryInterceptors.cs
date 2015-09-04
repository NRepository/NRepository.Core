namespace NRepository.Core
{
    using NRepository.Core.Command;
    using NRepository.Core.Query;

    public interface IRepositoryInterceptors :
            ICommandInterceptors,
            IQueryInterceptors
    {
    }
}
