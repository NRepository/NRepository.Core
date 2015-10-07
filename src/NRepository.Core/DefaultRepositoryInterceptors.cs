namespace NRepository.Core
{
    using NRepository.Core.Command;
    using NRepository.Core.Query;
    using NRepository.Core.Utilities;
    
    public sealed class DefaultRepositoryInterceptors : RepositoryInterceptors
    {
        public DefaultRepositoryInterceptors()
        {
        }

        public DefaultRepositoryInterceptors(IQueryInterceptor queryInterceptor)
        {
            Check.NotNull(queryInterceptor, "queryInterceptor");

            QueryInterceptor = queryInterceptor;
        }

        public DefaultRepositoryInterceptors(IAddCommandInterceptor addCommandInterceptor)
        {
            Check.NotNull(addCommandInterceptor, "addCommandInterceptor");

            AddCommandInterceptor = addCommandInterceptor;
        }

        public DefaultRepositoryInterceptors(IDeleteCommandInterceptor deleteCommandInterceptor)
        {
            Check.NotNull(deleteCommandInterceptor, "deleteCommandInterceptor");

            DeleteCommandInterceptor = deleteCommandInterceptor;
        }

        public DefaultRepositoryInterceptors(IModifyCommandInterceptor modifyCommandInterceptor)
        {
            Check.NotNull(modifyCommandInterceptor, "modifyCommandInterceptor");

            ModifyCommandInterceptor = modifyCommandInterceptor;
        }

        public DefaultRepositoryInterceptors(ISaveCommandInterceptor saveCommandInterceptor)
        {
            Check.NotNull(saveCommandInterceptor, "saveCommandInterceptor");

            SaveCommandInterceptor = saveCommandInterceptor;
        }
    }
}
