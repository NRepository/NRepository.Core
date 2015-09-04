namespace NRepository.Core.Command
{
    using NRepository.Core.Events;
    using System.Threading.Tasks;

    public interface ICommandRepository : IRepositoryContext, IRepositoryCommandEventHandler
    {
        void Add<T>(T entity) where T : class;

        void Add<T>(T entity, IAddCommandInterceptor addInterceptor) where T : class;

        void Modify<T>(T entity) where T : class;

        void Modify<T>(T entity, IModifyCommandInterceptor modifyInterceptor) where T : class;

        void Delete<T>(T entity) where T : class;

        void Delete<T>(T entity, IDeleteCommandInterceptor deleteInterceptor) where T : class;

        int Save();

        int Save(ISaveCommandInterceptor savingStrategy);

        Task AddAsync<T>(T entity) where T : class;

        Task AddAsync<T>(T entity, IAddCommandInterceptor addInterceptor) where T : class;

        Task ModifyAsync<T>(T entity) where T : class;

        Task ModifyAsync<T>(T entity, IModifyCommandInterceptor modifyInterceptor) where T : class;

        Task DeleteAsync<T>(T entity) where T : class;

        Task DeleteAsync<T>(T entity, IDeleteCommandInterceptor deleteInterceptor) where T : class;

        Task<int> SaveAsync();

        Task<int> SaveAsync(ISaveCommandInterceptor savingStrategy);
    }
}