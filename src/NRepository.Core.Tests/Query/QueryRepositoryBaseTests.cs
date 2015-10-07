namespace NRepository.Core.Tests.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using NRepository.Core;
    using NRepository.Core.Events;
    using NRepository.Core.Query;
    using NRepository.Core.Query.Specification;
    using NRepository.Core.Tests;
    using NRepository.TestKit;
    using NUnit.Framework;

    [TestFixture]
    public class QueryRepositoryBaseTests
    {
        private ISpecificationQueryStrategy<Person> ExpressionSpecificationStrategy = new ExpressionSpecificationQueryStrategy<Person>(p => p.Id == Names.IsabelleOsborne);
        private ISpecificationQueryStrategy<Parent> ParentExpressionSpecificationStrategy = new ExpressionSpecificationQueryStrategy<Parent>(p => p.Id == Names.IsabelleOsborne);
        private Type DefaultSpecificationStrategyType = typeof(DefaultSpecificationQueryStrategy<Person>);

        private static readonly IEnumerable<Person> Persons = PersonsData.Data;

        [Test]
        public void GetEntity()
        {
            Expression<Func<Person, bool>> isabelleExpression = p => p.Id == Names.IsabelleOsborne;

            var recorder = new InMemoryRecordedQueryRepository(PersonsData.Data);

            //recorder.GetEntity<Person>(params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(isabelleExpression), recorder, defaultQueryStrategy: typeof(AggregateQueryStrategy));

            //recorder.GetEntity<Person>(ISpecificationQueryStrategy<T> specification);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy), recorder, defaultQueryStrategy: typeof(AggregateQueryStrategy));

            //recorder.GetEntity<Person>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy, false), recorder, throwSearchException: false);

            //recorder.GetEntity<Person>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(isabelleExpression, new OrderByQueryStrategy<Person>(), false), recorder,  throwSearchException: false);

            //recorder.GetEntity<Person>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(isabelleExpression, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder);

            //recorder.GetEntity<Person>(ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy, false), recorder, throwSearchException: false);

            //recorder.GetEntity<Person>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy, new OrderByQueryStrategy<Person>(), false), recorder, throwSearchException: false);

            //recorder.GetEntity<Person>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder);

            // public T GetEntity<T>(params IQueryStrategy[] queryStrategies) where T : class
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, expectException: true);

        }
        
        [Test]
        public void GetEntities()
        {
            Expression<Func<Parent, bool>> isabelleExpression = p => p.Id == Names.IsabelleOsborne;
            // var additionalQueryData = "DummyData";

            var recorder = new InMemoryRecordedQueryRepository(Persons);
            var reverseStrategy = new MaterialiseQueryStrategy();
            var reverseStrategy2 = new MaterialiseQueryStrategy();

            //public IQueryable<T> GetEntities<T>();
            CallAndAssertGetEntities(() => recorder.GetEntities<Person>(), recorder, expectedCount: 11);
            CallAndAssertGetEntities(() => recorder.GetEntities<Child>(), recorder, expectedCount: 5);

            //public IQueryable<T> GetEntities<T>(params IQueryStrategy[] queryStrategy);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(reverseStrategy, reverseStrategy2), recorder, queryStrategyType: typeof(AggregateQueryStrategy));

            //public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(isabelleExpression), recorder, expectedCount: 1, queryStrategyType: typeof(ExpressionSpecificationQueryStrategy<Parent>));

            //public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(isabelleExpression, reverseStrategy, reverseStrategy2), recorder, queryStrategyType: typeof(AggregateQueryStrategy), expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(ParentExpressionSpecificationStrategy), recorder, expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(ParentExpressionSpecificationStrategy, reverseStrategy, reverseStrategy2), recorder, queryStrategyType: typeof(AggregateQueryStrategy), expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(ParentExpressionSpecificationStrategy, reverseStrategy, reverseStrategy2), recorder, queryStrategyType: typeof(AggregateQueryStrategy), expectedCount: 1);
        }

        [Test]
        public void CheckEntitySearchRepositoryExceptionThrown()
        {
            var recorder = new InMemoryRecordedQueryRepository(Persons);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(
                p => p.FirstName == "NotFound"),
                recorder,
                expectException: true,
                defaultQueryStrategy: typeof(AggregateQueryStrategy));

        }

        public void GetEntityASync()
        {
            //public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate);
            //public Task<T> GetEntityAsync<T>(params IQueryStrategy[] queryStrategies);
            //public Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification);
            //public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound);
            //public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
            //public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            //public Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound);
            //public Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
            //public Task<T> GetEntityAsync<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
        }

        public void GetEntityWithInterceptionDataAsync()
        {
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate);
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, params IQueryStrategy[] queryStrategies);
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification);
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound);
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound);
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            //public Task<T> GetEntityAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
        }

        public void GetEntityAsync()
        {
            //public Task<List<T>> GetEntitiesAsync<T>();
            //public Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData);
            //public Task<List<T>> GetEntitiesAsync<T>(params IQueryStrategy[] queryStrategy);
            //public Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate);
            //public Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            //public Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification);
            //public Task<List<T>> GetEntitiesAsync<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            //public Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, params IQueryStrategy[] queryStrategy);
            //public Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate);
            //public Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            //public Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification);
            //public Task<List<T>> GetEntitiesAsync<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
        }


        private Person CallAndAssertGetEntity(
            Func<Person> getEntity,
            InMemoryRecordedQueryRepository recorder,
            //Type specificationStrategy = null,
            //Type queryStrategy = null,
            bool throwSearchException = true,
//            object additionalQueryData = null,
            bool expectException = false,
            Type defaultQueryStrategy = null)
        {
            if (defaultQueryStrategy == null)
                defaultQueryStrategy = typeof(DefaultQueryStrategy);

            var person = default(Person);
            try
            {
                person = getEntity.Invoke();
                person.Id.ShouldEqual(Persons.Single(p => p.Id == Names.IsabelleOsborne).Id);
                expectException.ShouldEqual(false);
            }
            catch (EntitySearchRepositoryException)
            {
                expectException.ShouldEqual(true);
            }

            var qEvent = (SimpleRepositoryQueryEvent)recorder.QueryEvents.Single();
            qEvent.ThrowExceptionIfZeroOrManyFound.ShouldEqual(throwSearchException);
//            qEvent.QueryStrategy.GetType().ShouldEqual(queryStrategy ?? defaultQueryStrategy);
//            qEvent.AdditionalQueryData.ShouldEqual(additionalQueryData);

            recorder.QueryEvents.Clear();
            return person;
        }

        private IEnumerable<Person> CallAndAssertGetEntities(
            Func<IQueryable<Person>> getEntities,
            InMemoryRecordedQueryRepository recorder,
            Type queryStrategyType = null,
            object additionalQueryData = null,
            int expectedCount = 6)
        {
            queryStrategyType = queryStrategyType ?? typeof(DefaultQueryStrategy);
            var persons = default(IEnumerable<Person>);

            persons = getEntities.Invoke().ToList();
            persons.Count().ShouldEqual(expectedCount);

            var qEvent = (SimpleRepositoryQueryEvent)recorder.QueryEvents.Single();
//            qEvent.QueryStrategy.GetType().ShouldEqual(queryStrategyType);
            qEvent.AdditionalQueryData.ShouldEqual(additionalQueryData);

            recorder.QueryEvents.Clear();
            return persons;
        }
    }
}
