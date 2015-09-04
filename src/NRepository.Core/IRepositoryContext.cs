namespace NRepository.Core
{
    using System;

    public interface IRepositoryContext : IDisposable
    {
        object ObjectContext { get; }
    }
}
