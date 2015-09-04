namespace NRepository.Core.Query.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using NRepository.Core.Tests;

    [TestFixture()]
    public class ReverseQueryStrategyTests
    {
        [Test]
        public void CheckReverse()
        {
            var simpleEntities = SimpleEntity.CreateSimpleEntities();
            var results = simpleEntities.AddQueryStrategy(new ReverseQueryStrategy());

            results.First().Id.ShouldEqual(simpleEntities.Last().Id);
        }
    }
}
