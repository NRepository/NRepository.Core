namespace NRepository.Core.Events
{
    public class DefaultEntityAddedHandler : IRepositorySubscribe<EntityAddedEvent>
    {
        public void Handle(EntityAddedEvent details)
        {
        }
    }
}
