namespace NRepository.Core.Command
{
    using NRepository.Core.Utilities;
    using System;

    public class DefaultDeleteCommandInterceptor : IDeleteCommandInterceptor
    {
        public void Delete<T>(ICommandRepository repository, Action<T> deleteAction, T entity) where T : class
        {
            Check.NotNull(repository, "repository");
            Check.NotNull(deleteAction, "deleteAction");
            Check.NotNull(entity, "entity");
           
            deleteAction.Invoke(entity);
        }
    }
}
