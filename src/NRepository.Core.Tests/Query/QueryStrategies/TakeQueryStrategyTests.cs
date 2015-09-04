namespace NRepository.Core.Query.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using NRepository.Core.Tests;

    [TestFixture()]
    public class TakeQueryStrategyTests
    {
        [Test]
        public void CheckConstructors()
        {
            var query = new TakeQueryStrategy(2);
            query.Take.ShouldEqual(2);

            Assert.Throws<ArgumentException>(() => new TakeQueryStrategy(0));
        }

        [Test]
        public void CheckResults()
        {
            // Arrange
            var entities = SimpleEntity.CreateSimpleEntities();

            // Act
            var query = new TakeQueryStrategy(2);
            var results = entities.AddQueryStrategy(query).ToList();

            // Assert
            results.Count().ShouldEqual(2);
        }
    }
}