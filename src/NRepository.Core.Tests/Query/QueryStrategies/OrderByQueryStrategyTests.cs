namespace NRepository.Core.Query.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NRepository.Core.Query;
    using NUnit.Framework;
    using NRepository.Core.Tests;

    [TestFixture]
    public class OrderByStrategyTests
    {
        [Test]
        public void CalledWithNoDataDoesNotThrow()
        {
            // Arrange
            var repositoryViews = new InMemoryQueryRepository(FamilyTestData.GetData());


            // Act
            var entity1 = repositoryViews.GetEntities<Person>(
                new OrderByQueryStrategy(PersonIncludes.SortValue));

            var entity2 = repositoryViews.GetEntities<Person>(
                new OrderByDescendingQueryStrategy(PersonIncludes.SortValue));

            var entity3 = repositoryViews.GetEntities<Person>(
                new AggregateQueryStrategy(
                    new OrderByQueryStrategy(PersonIncludes.SortValue),
                    new ThenByQueryStrategy(PersonIncludes.Id)));

            var entity4 = repositoryViews.GetEntities<Person>(
                new AggregateQueryStrategy(
                    new OrderByDescendingQueryStrategy(PersonIncludes.SortValue),
                    new OrderByDescendingQueryStrategy(PersonIncludes.Id)));

            // Assert
            entity1.First().SortValue.ShouldEqual("A");
            entity2.First().SortValue.ShouldEqual("Z");
            entity3.First().SortValue.ShouldEqual("A");
            entity4.First().SortValue.ShouldEqual("J");
        }

        [Test]
        public void OrderByTest()
        {
            var repository = new InMemoryQueryRepository(FamilyTestData.GetData());
            var entities = repository.GetEntities<Person>(
                new OrderByQueryStrategy<Person>(p => p.SortValue));

            entities.First().SortValue.ShouldEqual("A");
            entities.Last().SortValue.ShouldEqual("Z");
       }


        [Test]
        public void OrderByTest2()
        {
            // Arrange
            var repositoryViews = new InMemoryQueryRepository(FamilyTestData.GetData());

            // Act
            var entities = repositoryViews.GetEntities<Person>(
                new OrderByQueryStrategy<Person>(p => p.SortValue));

            // Assert
            Assert.IsTrue(entities.First().SortValue == "A");
            Assert.IsTrue(entities.Last().SortValue == "Z");
        }

        [Test]
        public void OrderByWithSecondParamterTest()
        {
            // Arrange
            var repositoryViews = new InMemoryQueryRepository(FamilyTestData.GetData());

            // Act
            var entities = repositoryViews.GetEntities<Person>(new OrderByQueryStrategy(PersonIncludes.Title, PersonIncludes.SortValue));
        }

        [Test]
        public void OrderByDescendingTest()
        {
            // Arrange
            var repositoryViews = new InMemoryQueryRepository(FamilyTestData.GetData());

            // Act
            var entities = repositoryViews.GetEntities<Person>(new OrderByQueryStrategy(PersonIncludes.SortValue));
        }

        public void OrderByDescendingWithSecondParamterTest()
        {
            // Arrange
            var repositoryViews = new InMemoryQueryRepository(FamilyTestData.GetData());

            // Act
            var entities = repositoryViews.GetEntities<Person>(new OrderByQueryStrategy(PersonIncludes.SortValue));
        }

        [Test]
        public void OrderByDescendingAndThenByTest()
        {
          
        }

        [Test]
        public void OrderByAndThenByTest()
        {

          
        }

        [Test]
        public void OrderDescendingByAndThenByDescendingTest()
        {
           
        }
    }
}
