namespace NRepository.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EntityRepositoryException : RepositoryException
    {
        public EntityRepositoryException()
        {
        }

        public EntityRepositoryException(string message)
            : base(message)
        {
        }

        public EntityRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}