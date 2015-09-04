namespace NRepository.Core.Query
{
    public interface IQueryStrategy : IQueryableRepository
    {
        string Identifier { get; }
        IQueryableRepository QueryableRepository { get; set; }
    }
}
