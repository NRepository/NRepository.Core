namespace NRepository.Core.Events
{
    public class DefaultEntityDeletedHandler : IRepositorySubscribe<EntityDeletedEvent>
    {
        public void Handle(EntityDeletedEvent details)
        {
        }
    }
}
