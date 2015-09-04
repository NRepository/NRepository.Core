namespace NRepository.Core.Query.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Query;
    using TestKit;

    [TestFixture()]
    public class MultipleTextSearchSpecificationStrategyTests
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
            var items = repository.GetEntities(
                new MultipleTextSearchSpecificationStrategy<Item>("x", p => p.Item1, p => p.Item2));

            items.Count().ShouldEqual(2);
        }
    }
}
