namespace NRepository.Core.Query.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using NRepository.Core.Tests;

    [TestFixture]
    public class MaterialiseQueryStrategyTests
    {
        [Test]
        public void CheckAllEntitiesReturned()
        {
            var simpleEntities = SimpleEntity.CreateSimpleEntities();
            var query = new MaterialiseQueryStrategy();
            simpleEntities.AddQueryStrategy(query).Count().ShouldEqual(simpleEntities.Count());
        }
    }
}
