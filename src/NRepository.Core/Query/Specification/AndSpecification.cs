namespace NRepository.Core.Query.Specification
{
    using NRepository.Core.Utilities;
    using System;
    using System.Linq.Expressions;

    public sealed class AndSpecification<T>
       : CompositeSpecification<T>
        where T : class
    {
        public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            Check.NotNull(leftSide, "leftSide");
            Check.NotNull(rightSide, "rightSide");

            this.LeftSideSpecification = leftSide;
            this.RightSideSpecification = rightSide;
        }

        public override ISpecification<T> LeftSideSpecification
        {
            get;
            protected set;
        }

        public override ISpecification<T> RightSideSpecification
        {
            get;
            protected set;
        }

        /// <summary>
        /// Returns the lambda expression that must be satisfied by the objects matching this specification.
        /// </summary>
        /// <returns>The lambda expression.</returns>
        public override Expression<Func<T, bool>> SatisfiedBy(object additionalQueryData)
        {
            Expression<Func<T, bool>> left = this.LeftSideSpecification.SatisfiedBy(additionalQueryData);
            Expression<Func<T, bool>> right = this.RightSideSpecification.SatisfiedBy(additionalQueryData);

            return left.And(right);
        }
    }
}
