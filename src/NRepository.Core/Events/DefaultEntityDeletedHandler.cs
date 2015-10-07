namespace NRepository.Core.Events
{
    public sealed class DefaultEntityDeletedHandler : IRepositorySubscribe<EntityDeletedEvent>
    {
        public void Handle(EntityDeletedEvent details)
        {
        }
    }
}
