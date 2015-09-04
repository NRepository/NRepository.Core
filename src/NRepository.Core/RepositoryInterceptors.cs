namespace NRepository.Core
{
    using NRepository.Core.Command;
    using NRepository.Core.Query;

    public abstract class RepositoryInterceptors : IRepositoryInterceptors
    {
        public RepositoryInterceptors()
        {
            QueryInterceptor = new DefaultQueryInterceptor();
            AddCommandInterceptor = new DefaultAddCommandInterceptor();
            ModifyCommandInterceptor = new DefaultModifyCommandInterceptor();
            DeleteCommandInterceptor = new DefaultDeleteCommandInterceptor();
            SaveCommandInterceptor = new DefaultSaveCommandInterceptor();
        }

        public IQueryInterceptor QueryInterceptor
        {
            get;
            set;
        }

        public IAddCommandInterceptor AddCommandInterceptor
        {
            get;
            set;
        }

        public IModifyCommandInterceptor ModifyCommandInterceptor
        {
            get;
            set;
        }

        public IDeleteCommandInterceptor DeleteCommandInterceptor
        {
            get;
            set;
        }

        public ISaveCommandInterceptor SaveCommandInterceptor
        {
            get;
            set;
        }
    }
}
