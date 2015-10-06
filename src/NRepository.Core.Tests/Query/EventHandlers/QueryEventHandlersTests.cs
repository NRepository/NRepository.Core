namespace NRepository.Core.Query.Tests
{
    using System.Linq;
    using NRepository.Core.Query.Specification;
    using NRepository.Core.Tests;
    using NUnit.Framework;
    using NRepository.Core.Events;
    using System.Collections.Generic;

    [TestFixture()]
    public class QueryEventHandlersTests
    {
        public class TestRepositoryQueriedEvent : IRepositorySubscribe<RepositoryQueryEvent>
        {
            public List<RepositoryQueryEvent> QueriedEventList { get; private set; }

            public TestRepositoryQueriedEvent()
            {
                QueriedEventList = new List<RepositoryQueryEvent>();
            }

            public void Handle(RepositoryQueryEvent repositoryEvent)
            {
                QueriedEventList.Add(repositoryEvent);
            }

            public void ResetQueriedEventList()
            {
                QueriedEventList.Clear();
            }
        }

        //[Test]
        public void CheckSimpleEntitiesQueryEventHandler()
        {
            // Arrange
            var eventHandler = new TestRepositoryQueriedEvent();
            var repository = new FamilyQueryRepository(new QueryEventHandler(eventHandler));

            // Act
            repository.GetEntities<Person>();
            var queryEvent = (GetEntitiesRepositoryQueryEvent)eventHandler.QueriedEventList.Single();

            // Assert
            Assert.IsInstanceOf<DefaultQueryStrategy>(queryEvent.QueryStrategy);
            Assert.IsTrue(queryEvent.QueryStrategy == null);
            Assert.IsNull(queryEvent.ThrowExceptionIfZeroOrManyFound);
        }

        [Test]
        public void CheckQueryEntitiesEventHandlerWithSpecification()
        {
            // Arrange
            var eventHandler = new TestRepositoryQueriedEvent();
            var repository = new FamilyQueryRepository(new QueryEventHandler(eventHandler));

            // Act
            repository.GetEntities<Person>(p => p.Id == Names.AimmeOsborne);
            var queryEvent = (GetEntitiesRepositoryQueryEvent)eventHandler.QueriedEventList.Single();

            // Assert
            Assert.IsInstanceOf<ExpressionSpecificationQueryStrategy<Person>>(queryEvent.QueryStrategy);
            Assert.IsNull(queryEvent.ThrowExceptionIfZeroOrManyFound);
        }

        [Test]
        public void CheckSingleQueryEventHandlerThrowExceptionFlagFalse()
        {
            // Arrange
            var eventHandler = new TestRepositoryQueriedEvent();
            var repository = new FamilyQueryRepository(new QueryEventHandler(eventHandler));

            // Act
            var entity = repository.GetEntity<Person>(p => p.Id == Names.AimmeOsborne, false);
            var queryEvent = (GetEntityRepositoryQueryEvent)eventHandler.QueriedEventList.Single();

            // Assert
            Assert.IsNotNull(entity);
            Assert.IsFalse(queryEvent.ThrowExceptionIfZeroOrManyFound.Value);
        }
    }
}
