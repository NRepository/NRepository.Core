namespace NRepository.Core
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NRepository.Core.Utilities;

    [ExcludeFromCodeCoverage]
    public sealed class EntityUpdateRepositoryException : EntityRepositoryException
    {
        public EntityUpdateRepositoryException(Dictionary<string, string> saveErrors)
        {
            Check.NotNull(saveErrors, "saveErrors");
            
            Errors = saveErrors;
        }

        public EntityUpdateRepositoryException(Dictionary<string, string> saveErrors, Exception innerException)
            : base(string.Empty, innerException)
        {
            Check.NotNull(saveErrors, "saveErrors");

            Errors = saveErrors;
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
                    Errors.Select(item => string.Format("Update Error: {{ Key: {0}, Value: {1}}}", item.Key, item.Value)));

                return retVal;
            }
        }
    }
}
