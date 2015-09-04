namespace NRepository.Core.Query.Specification
{
    public interface ISpecificationQueryStrategy<T> : 
        IQueryStrategy,
        ISpecification<T>
    {
    }
}
