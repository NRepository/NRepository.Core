namespace BluePear.Repository.CoreTests.Command
{
    using NRepository.Core.Command;
    using NRepository.Core.Tests;
    using NUnit.Framework;
    using System;

    public class SaveInterceptor : ISaveCommandInterceptor
    {
        public bool ThrowOriginalException
        {
            get { return true; }
        }

        public int Save(ICommandRepository repository, Func<int> saveFunc)
        {
            return 0;
        }
    }

    public class AddCommandInterceptor : IAddCommandInterceptor
    {
        public void Add<T>(ICommandRepository repository, Action<T> addAction, T entity) where T : class
        {
            addAction(entity);
        }
    }


    [TestFixture]
    public class MethodInterceptors
    {
        [Test]
        public void CheckSaveInterceptor()
        {
            var commandRepository = new InMemoryCommandRepository();
            commandRepository.Add(new Parent(), new AddCommandInterceptor());
            commandRepository.Save(new SaveInterceptor());
        }
    }
}
