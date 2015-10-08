namespace NRepository.Core.Query.Specification
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public abstract class SpecificationQueryStrategy<TEntity> : ISpecificationQueryStrategy<TEntity> where TEntity : class
    {
        public string Identifier
        {
            get;
            protected set;
        }

        public IQueryableRepository QueryableRepository
        {
            get;
            set;
        }

        /// <summary>
        ///  And operator.
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this AND operation</param>
        /// <param name="rightSideSpecification">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static SpecificationQueryStrategy<TEntity> operator &(SpecificationQueryStrategy<TEntity> leftSideSpecification, SpecificationQueryStrategy<TEntity> rightSideSpecification)
        {
            Check.NotNull(leftSideSpecification, "leftSideSpecification");
            Check.NotNull(rightSideSpecification, "rightSideSpecification");

            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Or operator.
        /// </summary>
        /// <param name="leftSideSpecification">Left operand in this OR operation.</param>
        /// <param name="rightSideSpecification">Right operand in this OR operation.</param>
        /// <returns>New specification.</returns>
        public static SpecificationQueryStrategy<TEntity> operator |(SpecificationQueryStrategy<TEntity> leftSideSpecification, SpecificationQueryStrategy<TEntity> rightSideSpecification)
        {
            Check.NotNull(leftSideSpecification, "leftSideSpecification");
            Check.NotNull(rightSideSpecification, "rightSideSpecification");

            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Not operator.
        /// </summary>
        /// <param name="specification">Specification to negate</param>
        /// <returns>New specification</returns>
        public static SpecificationQueryStrategy<TEntity> operator !(SpecificationQueryStrategy<TEntity> specification)
        {
            Check.NotNull(specification, "specification");

            return new NotSpecification<TEntity>(specification);
        }

        /// <summary>
        /// Returns the lambda expression that must be satisfied by the objects matching this specification.
        /// </summary>
        /// <returns>The lambda expression.</returns>
        public abstract Expression<Func<TEntity, bool>> SatisfiedBy(object additionalQueryData);

        public Dictionary<string, string> SpecificationDetails
        {
            get
            {
                var dict = new Dictionary<string, string>();

                try
                {
                    var bf = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
                    dict["Lambda expression"] = SatisfiedBy(null).Body.ToString();
                    GetType().GetFields(bf).ToList().ForEach(p => dict[p.Name] = p.GetValue(this).ToString());
                }
                catch
                {
                    // Intentionally blank.
                }

                return dict;
            }
        }

        public IQueryable<T> GetQueryableEntities<T>(object additionalQueryData) where T : class
        {
            var cast = (IQueryable<TEntity>)QueryableRepository.GetQueryableEntities<T>(additionalQueryData);
            var result = Queryable.Where(cast, SatisfiedBy(additionalQueryData));
            return (IQueryable<T>)result;
        }

        //  User-defined conversion from double to Digit 
        public static implicit operator QueryStrategy(SpecificationQueryStrategy<TEntity> specification)
        {
            Check.NotNull(specification, "specification");

            return (QueryStrategy)specification;
        }
    }
}