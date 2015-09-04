namespace NRepository.Core.Query.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using NRepository.Core.Tests;

    [TestFixture()]
    public class SkipQueryStrategyTests
    {
        [Test]
        public void CheckConstructors()
        {
            var query = new SkipQueryStrategy(2);
            query.Skip.ShouldEqual(2);

            Assert.Throws<ArgumentException>(() => new SkipQueryStrategy(0));
        }

        [Test]
        public void CheckResults()
        {
            // Arrange
            var entities = SimpleEntity.CreateSimpleEntities();

            // Act
            var query = new SkipQueryStrategy(2);
            var results = entities.AddQueryStrategy(query).ToList();

            // Assert
            results.First().Id.ShouldEqual(3);
        }
    }
}
