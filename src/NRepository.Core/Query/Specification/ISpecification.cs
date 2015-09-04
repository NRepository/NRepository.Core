namespace NRepository.Core.Query.Specification
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Base contract for Specification pattern, for more information
    /// about this pattern see http://martinfowler.com/apsupp/spec.pdf
    /// or http://en.wikipedia.org/wiki/Specification_pattern.
    /// This is really a variant implementation where we have added Linq and
    /// lambda expression into the pattern.
    /// </summary>
    /// <typeparam name="T">Type of entity this specification applies to.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Returns the lambda expression that must be satisfied by the objects matching this specification.
        /// </summary>
        /// <returns>The lambda expression.</returns>
        Expression<Func<T, bool>> SatisfiedBy(object additionalQueryData);

        /// <summary>
        /// Returns a detailed list of the specification details of the object.
        /// </summary>
        /// <value>The specification details.</value>
        Dictionary<string, string> SpecificationDetails { get; }
    }
}
