namespace NRepository.Core
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NRepository.Core.Utilities;

    [ExcludeFromCodeCoverage]
    public sealed class EntityValidationRepositoryException : EntityRepositoryException
    {
        public EntityValidationRepositoryException(Dictionary<string, string> validationErrors)
        {
            Check.NotNull(validationErrors, "validationErrors");

            Errors = validationErrors;
        }

        public EntityValidationRepositoryException(Dictionary<string, string> validationErrors, Exception innerException)
            : base(string.Empty, innerException)
        {
            Check.NotNull(validationErrors, "validationErrors");

            Errors = validationErrors;
        }

        public Dictionary<string, string> Errors
        {
            get;
            private set;
        }

        public override string Message
        {
            get
            {
                var retVal = string.Join(
                     Environment.NewLine,
                     Errors.Select(item => string.Format("Validation Error: {{ Key: {0}, Value: {1}}}", item.Key, item.Value)));
                
                return retVal;
            }
        }
    }
}
