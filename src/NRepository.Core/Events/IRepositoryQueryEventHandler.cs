namespace NRepository.Core.Events
{
    public interface IRepositoryQueryEventHandler
    {
        void RaiseEvent<T>(T evnt) where T : class, IRepositoryQueryEvent;
    }
}