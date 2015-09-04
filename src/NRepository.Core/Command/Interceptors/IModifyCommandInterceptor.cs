namespace NRepository.Core.Command
{
    using System;

    public interface IModifyCommandInterceptor : ICommandInterceptor
    {
        void Modify<T>(ICommandRepository repository, Action<T> modifyAction, T entity) where T : class;
    }
}