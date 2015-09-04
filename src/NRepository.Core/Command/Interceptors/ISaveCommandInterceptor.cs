namespace NRepository.Core.Command
{
    using System;

    public interface ISaveCommandInterceptor : ICommandInterceptor
    {
        bool ThrowOriginalException { get; }

        int Save(ICommandRepository repository, Func<int> saveFunc);
    }
}