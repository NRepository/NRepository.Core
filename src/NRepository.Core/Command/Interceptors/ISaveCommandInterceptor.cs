namespace NRepository.Core.Command
{
    using System;

    public interface ISaveCommandInterceptor : ICommandInterceptor
    {
        int Save(ICommandRepository repository, Func<int> saveFunc);
    }
}