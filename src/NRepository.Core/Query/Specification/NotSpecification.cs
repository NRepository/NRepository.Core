namespace NRepository.Core.Query.Specification
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public sealed class NotSpecification<T>
        : SpecificationQueryStrategy<T>
        where T : class
    {
        private readonly Expression<Func<T, bool>> originalCriteria;

        public NotSpecification(ISpecification<T> originalSpecification)
        {
            if (originalSpecification == null)
                throw new ArgumentNullException("originalSpecification");

            this.originalCriteria = originalSpecification.SatisfiedBy(null);
        }

        public override Expression<Func<T, bool>> SatisfiedBy(object additionalQueryData)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(this.originalCriteria.Body), this.originalCriteria.Parameters.Single());
        }
    }
}
