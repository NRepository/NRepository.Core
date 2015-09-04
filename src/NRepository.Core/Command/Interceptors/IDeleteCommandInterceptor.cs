namespace NRepository.Core.Command
{
    using System;

    public interface IDeleteCommandInterceptor : ICommandInterceptor
    {
        void Delete<T>(ICommandRepository repository, Action<T> deleteAction, T entity) where T : class;
    }
}
