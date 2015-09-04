namespace NRepository.Core.Query.Specification
{
    using System;
    using System.Linq.Expressions;

    public sealed class OrSpecification<T>
          : CompositeSpecification<T>
          where T : class
    {
        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == null)
            {
                throw new ArgumentNullException("leftSide");
            }

            if (rightSide == null)
            {
                throw new ArgumentNullException("rightSide");
            }

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

        public override Expression<Func<T, bool>> SatisfiedBy(object additionalQueryData)
        {
            Expression<Func<T, bool>> left = this.LeftSideSpecification.SatisfiedBy(additionalQueryData);
            Expression<Func<T, bool>> right = this.RightSideSpecification.SatisfiedBy(additionalQueryData);

            return left.Or(right);
        }
    }
}
