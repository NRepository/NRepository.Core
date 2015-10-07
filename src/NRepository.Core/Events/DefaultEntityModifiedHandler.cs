namespace NRepository.Core.Events
{
    public sealed class DefaultEntityModifiedHandler : IRepositorySubscribe<EntityModifiedEvent>
    {
        public void Handle(EntityModifiedEvent details)
        {
        }
    }
}
