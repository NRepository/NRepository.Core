namespace NRepository.Core
{
    using NRepository.Core.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public sealed class EntitySearchRepositoryException : EntityRepositoryException
    {
        public EntitySearchRepositoryException(int rowsFound, string entityName, params string[] parameters)
        {
            Check.NotEmpty(entityName, "entityName");
            Check.NotNull(parameters, "parameters");

            RowsFound = rowsFound;
            EntityName = entityName;

            SearchParameters = new Dictionary<string, string>();
            //for (int i = 0; i < parameters.Length; i += 2)
            //{
            //    SearchParameters.Add(parameters[i], parameters[i + 1]);
            //}

            SearchParameters.Add("Rows found", RowsFound.ToString());
        }

        public EntitySearchRepositoryException(int rowsFound, string entityName, Dictionary<string, string> searchParameters)
        {
            Check.NotEmpty(entityName, "entityName");
            Check.NotNull(searchParameters, "searchParameters");
            
            RowsFound = rowsFound;
            EntityName = entityName;
            SearchParameters = searchParameters;

            SearchParameters.Add("Rows found", RowsFound.ToString());
        }

        public EntitySearchRepositoryException(int rowsFound, string entityName, Dictionary<string, string> searchParameters, Exception innerException)
            : base(string.Empty, innerException)
        {
            Check.NotEmpty(entityName, "entityName");
            Check.NotNull(searchParameters, "searchParameters");

            RowsFound = rowsFound;
            EntityName = entityName;
            SearchParameters = searchParameters;

            SearchParameters.Add("Rows found", RowsFound.ToString());
        }

        public int RowsFound
        {
            get;
            private set;
        }

        public string EntityName
        {
            get;
            set;
        }

        public Dictionary<string, string> SearchParameters
        {
            get;
            set;
        }

        public override string Message
        {
            get
            {
                var errorMsg = string.Empty;
                foreach (var item in SearchParameters)
                {
                    errorMsg += string.Format("{{Key: {0}, Value: {1} }}, {2}", item.Key, item.Value, Environment.NewLine);
                }

                var retVal = string.Format("EntityName: {0}, Search Parameters: {1}", EntityName, errorMsg);
                return retVal;
            }
        }

        public static Dictionary<string, string> CreateSearchParameters(string key, string value)
        {
            Check.NotEmpty(key, "key");
            Check.NotEmpty(value, "value");

            var retVal = new Dictionary<string, string>();
            retVal.Add(key, value);
            return retVal;
        }
    }
}
