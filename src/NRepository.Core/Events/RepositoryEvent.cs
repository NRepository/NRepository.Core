namespace NRepository.Core.Events
{
    using System;

    public abstract class RepositoryEvent : IRepositoryEvent
    {
        public object AdditionalEventData
        {
            get;
            set;
        }
    }
}
