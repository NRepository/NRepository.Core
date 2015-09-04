namespace NRepository.Core.Query.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture()]
    public class PagingQueryStrategyTests
    {
        [Test]
        public void CheckConstructors()
        {
            new PagingQueryStrategy(0, 1).ShouldNotEqual(null);
            Assert.Throws<ArgumentException>(() => new PagingQueryStrategy(3, 0));
            Assert.Throws<ArgumentException>(() => new PagingQueryStrategy(-1, 2));

            var query = new PagingQueryStrategy(2, 3);
            query.PageSize.ShouldEqual(3);
            query.Page.ShouldEqual(2);
        }

        [Test]
        public void CheckResults()
        {
            // Arrange
            var entities = new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Act
            var query = new PagingQueryStrategy(2, 2);
            var results = entities.AddQueryStrategy(query).ToList();

            // Assert
            results.Count().ShouldEqual(2);
            results.First().ShouldEqual(3);
            results.Last().ShouldEqual(4);
            query.RowCount.HasValue.ShouldEqual(false);
        }


        [Test]
        public void CheckResultsWithCount()
        {
            // Arrange
            var entities = new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Act
            var query = new PagingQueryStrategy(2, 2, true);
            var results = entities.AddQueryStrategy(query).ToList();

            // Assert
            results.Count().ShouldEqual(2);
            results.First().ShouldEqual(3);
            results.Last().ShouldEqual(4);
            query.RowCount.HasValue.ShouldEqual(true);
            query.RowCount.ShouldEqual(9);
        }
    }
}
