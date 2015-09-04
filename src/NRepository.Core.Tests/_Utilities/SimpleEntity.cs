namespace NRepository.Core.Tests
{
    using System.Linq;

    public class SimpleEntity
    {
        public static IQueryable<SimpleEntity> CreateSimpleEntities()
        {
            return new[]
                {
                    new SimpleEntity(1,3),
                    new SimpleEntity(2,2),
                    new SimpleEntity(3,1),
                    new SimpleEntity(4,1),
                    new SimpleEntity(5,2),
                    new SimpleEntity(6,3),
                    new SimpleEntity(7,3),
                    new SimpleEntity(8,2),
                    new SimpleEntity(9,1),
                }.AsQueryable();
        }

        public SimpleEntity(int id, int groupId)
        {
            GroupId = groupId;
            Id = id;
        }

        public int Id
        {
            get;
            set;
        }

        public int GroupId
        {
            get;
            private set;
        }
    }
}
