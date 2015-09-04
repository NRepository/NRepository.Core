namespace NRepository.Core
{
    using NRepository.Core.Command;
    using NRepository.Core.Query;

    public interface IRepository : 
        IQueryRepository, 
        ICommandRepository
    {
        IQueryRepository QueryRepository { get; }

        ICommandRepository CommandRepository { get; }
    }
}