namespace NRepository.Core.Events
{
    public interface IRepositoryCommandEventHandler
    {
        void RaiseEvent<T>(T evnt) where T : class, IRepositoryCommandEvent;
    }
}
