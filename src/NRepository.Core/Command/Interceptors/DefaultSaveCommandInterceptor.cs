namespace NRepository.Core.Command
{
    using NRepository.Core.Utilities;
    using System;

    public class DefaultSaveCommandInterceptor : ISaveCommandInterceptor
    {
        public int Save(ICommandRepository repository, Func<int> saveFunc)
        {
            Check.NotNull(repository, "repository");
            Check.NotNull(repository, "saveFunc");

            var retVal = saveFunc.Invoke();
            return retVal;
        }

        public bool ThrowOriginalException
        {
            get { return false; }
        }
    }
}
