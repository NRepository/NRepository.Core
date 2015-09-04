namespace NRepository.Core.Command
{
    using NRepository.Core.Utilities;
    using System;

    public class DefaultAddCommandInterceptor : IAddCommandInterceptor
    {
        public void Add<T>(ICommandRepository repository, Action<T> add, T entity) where T : class
        {
            Check.NotNull(repository, "repository");
            Check.NotNull(add, "add");
            Check.NotNull(entity, "entity");
        
            add.Invoke(entity);
        }
    }
}
