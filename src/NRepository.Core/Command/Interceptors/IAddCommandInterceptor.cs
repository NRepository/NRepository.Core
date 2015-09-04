namespace NRepository.Core.Command
{
    using System;

    public interface IAddCommandInterceptor : ICommandInterceptor
    {
        void Add<T>(ICommandRepository repository, Action<T> addAction, T entity) where T : class;
    }
}