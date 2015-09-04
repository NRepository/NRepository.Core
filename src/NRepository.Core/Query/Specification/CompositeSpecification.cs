namespace NRepository.Core.Query.Specification
{
    public abstract class CompositeSpecification<T>
         : SpecificationQueryStrategy<T>
         where T : class
    {
        public abstract ISpecification<T> LeftSideSpecification { get; protected set; }

        public abstract ISpecification<T> RightSideSpecification { get; protected set; }
    }
}
