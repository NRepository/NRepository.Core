namespace NRepository.Core.Command
{
    using NRepository.Core.Utilities;
    using System;

    public class DefaultModifyCommandInterceptor : IModifyCommandInterceptor
    {
        public void Modify<T>(ICommandRepository repository, Action<T> modifyAction, T entity) where T : class
        {
            Check.NotNull(repository, "modifyAction");
            Check.NotNull(entity, "entity");

            modifyAction.Invoke(entity);
        }
    }
}
