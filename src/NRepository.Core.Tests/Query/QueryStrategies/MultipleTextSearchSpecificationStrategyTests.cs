namespace NRepository.Core.Query.Tests
{
    using NUnit.Framework;
    using Query;
    using System.Linq;
    using TestKit;

    [TestFixture()]
    public class TextSearchSpecificationStrategyTests
    {
        private class Item
        {
            public string Item1 { get; set; }
            public string Item2 { get; set; }

        }
        [Test]
        public void CheckFilter()
        {
            var entities = new[]
            {
                EntityGenerator.Create<Item>(p => p.Item1 = "xyz"),
                EntityGenerator.Create<Item>(p => p.Item2 = "xxx"),
                EntityGenerator.Create<Item>(),
                EntityGenerator.Create<Item>(),
            };

            var repository = new InMemoryRepository(entities);
            var items = repository.GetEntities<Item>(
                new TextSearchSpecificationStrategy<Item>("x", p => p.Item1, p => p.Item2));

            items.Count().ShouldEqual(2);
        }
    }
}
