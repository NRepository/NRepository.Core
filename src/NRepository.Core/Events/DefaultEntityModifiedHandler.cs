namespace NRepository.Core.Events
{
    public class DefaultEntityModifiedHandler : IRepositorySubscribe<EntityModifiedEvent>
    {
        public void Handle(EntityModifiedEvent details)
        {
        }
    }
}
