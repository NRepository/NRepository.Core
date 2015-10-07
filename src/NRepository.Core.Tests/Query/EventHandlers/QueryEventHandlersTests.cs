namespace NRepository.Core.Query.Tests
{
    using NRepository.Core.Events;
    using NRepository.Core.Tests;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture()]
    public class QueryEventHandlersTests
    {
        public class TestRepositoryQueriedEvent : IRepositorySubscribe<RepositoryQueryEvent>
        {
            public List<RepositoryQueryEvent> QueriedEventList { get; }

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
            var queryEvent = (SimpleRepositoryQueryEvent)eventHandler.QueriedEventList.Single();

            // Assert
            Assert.IsInstanceOf<DefaultQueryStrategy>(queryEvent.QueryStrategy);
            Assert.IsTrue(queryEvent.QueryStrategy == null);
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
            var queryEvent = (SimpleRepositoryQueryEvent)eventHandler.QueriedEventList.Single();

            // Assert
            Assert.IsNotNull(entity);
            Assert.IsFalse(queryEvent.ThrowExceptionIfZeroOrManyFound.Value);
        }
    }
}
