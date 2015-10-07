namespace NRepository.Core.Events
{
    public sealed class DefaultEntityAddedHandler : IRepositorySubscribe<EntityAddedEvent>
    {
        public void Handle(EntityAddedEvent details)
        {
        }
    }
}
