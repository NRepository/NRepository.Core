namespace NRepository.Core.Query
{
    using System;
    using System.Linq.Expressions; 
    using NRepository.Core.Query.Specification;

    public class DefaultSpecificationQueryStrategy<T> : SpecificationQueryStrategy<T> where T : class
    {
        public override Expression<Func<T, bool>> SatisfiedBy(object additionalQueryData)
        {
            return p => true;
        }
    }
}
