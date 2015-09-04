namespace NRepository.Core.Query.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture()]
    public class AggregateQueryStrategyTests
    {
        [Test]
        public void CheckExecptions()
        {
            Assert.Throws<ArgumentNullException>(() => new AggregateQueryStrategy(default(IEnumerable<IQueryStrategy>)));
            Assert.Throws<ArgumentException>(() => new AggregateQueryStrategy(default(IQueryStrategy)));
            Assert.Throws<ArgumentException>(() => new AggregateQueryStrategy(new IQueryStrategy[] { null }));
        }

        [Test]
        public void CheckAddAddsQueryStrategy()
        {
            var aggregate = new AggregateQueryStrategy();
            aggregate.Aggregates.Count().ShouldEqual(0);
            aggregate.Add(new DefaultQueryStrategy());
            aggregate.Aggregates.Count().ShouldEqual(1);
        }

        [Test]
        public void CheckCtors()
        {
            var strategy1 = new ReverseQueryStrategy();
            var strategy2 = new ReverseQueryStrategy();

            var aggregate = new AggregateQueryStrategy(strategy1, strategy2);
            aggregate.Aggregates.Count().ShouldEqual(2);

            aggregate = new AggregateQueryStrategy(new[] { strategy1, strategy2 }.ToList());
            aggregate.Aggregates.Count().ShouldEqual(2);
        }
    }
}
