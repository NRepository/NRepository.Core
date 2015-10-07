namespace NRepository.Core.Query.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
    }
}
