namespace NRepository.Core.Events
{
    public abstract class RepositoryEvent : IRepositoryEvent
    {
        public object AdditionalEventData
        {
            get;
            set;
        }
    }
}
