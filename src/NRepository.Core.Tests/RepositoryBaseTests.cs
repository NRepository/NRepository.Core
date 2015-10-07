namespace NRepository.Core.Tests.Query
{
    using NRepository.Core;
    using NRepository.Core.Events;
    using NRepository.Core.Query;
    using NRepository.Core.Query.Specification;
    using NRepository.Core.Tests;
    using NRepository.TestKit;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    [TestFixture]
    public class RepositoryBaseTests
    {
        private ISpecificationQueryStrategy<Person> ExpressionSpecificationStrategy = new ExpressionSpecificationQueryStrategy<Person>(p => p.Id == Names.IsabelleOsborne);
        private ISpecificationQueryStrategy<Parent> ParentExpressionSpecificationStrategy = new ExpressionSpecificationQueryStrategy<Parent>(p => p.Id == Names.IsabelleOsborne);
        private Type DefaultSpecificationStrategyType = typeof(DefaultSpecificationQueryStrategy<Person>);

        private static readonly ICollection<object> Persons = (ICollection<object>)PersonsData.Data.Cast<object>().ToList();

        [Test]
        public void GetEntity()
        {
            Expression<Func<Person, bool>> isabelleExpression = p => p.Id == Names.IsabelleOsborne;

            var recorder = new InMemoryRecordedRepository(Persons);

            //recorder.GetEntity<Person>(params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(isabelleExpression), recorder, defaultQueryStrategy: typeof(AggregateQueryStrategy));

            //recorder.GetEntity<Person>(ISpecificationQueryStrategy<T> specification);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy), recorder);

            //recorder.GetEntity<Person>(Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy, false), recorder, throwSearchException: false);

            //recorder.GetEntity<Person>(Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(isabelleExpression, new OrderByQueryStrategy<Person>(), false), recorder, queryStrategy: typeof(OrderByQueryStrategy<Person>), throwSearchException: false);

            //recorder.GetEntity<Person>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(isabelleExpression, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, queryStrategy: typeof(AggregateQueryStrategy));

            //recorder.GetEntity<Person>(ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy, false), recorder, throwSearchException: false);

            //recorder.GetEntity<Person>(ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy, new OrderByQueryStrategy<Person>(), false), recorder, queryStrategy: typeof(OrderByQueryStrategy<Person>), throwSearchException: false);

            //recorder.GetEntity<Person>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(ExpressionSpecificationStrategy, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, queryStrategy: typeof(AggregateQueryStrategy));

            // public T GetEntity<T>(params IQueryStrategy[] queryStrategies) where T : class
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, specificationStrategy: typeof(DefaultSpecificationQueryStrategy<Person>), queryStrategy: typeof(AggregateQueryStrategy), expectException: true);
        }



        [Test]
        public void GetEntities()
        {
            Expression<Func<Parent, bool>> isabelleExpression = p => p.Id == Names.IsabelleOsborne;
            // var additionalQueryData = "DummyData";

            var recorder = new InMemoryRecordedRepository(Persons);
            var reverseStrategy = new MaterialiseQueryStrategy();
            var reverseStrategy2 = new MaterialiseQueryStrategy();

            //public IQueryable<T> GetEntities<T>();
            CallAndAssertGetEntities(() => recorder.GetEntities<Person>(), recorder, specificationStrategy: DefaultSpecificationStrategyType, expectedCount: 11);
            CallAndAssertGetEntities(() => recorder.GetEntities<Child>(), recorder, specificationStrategy: typeof(DefaultSpecificationQueryStrategy<Child>), expectedCount: 5);

            //public IQueryable<T> GetEntities<T>(object additionalQueryData);
            //  CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(additionalQueryData), recorder, specificationStrategy: typeof(DefaultSpecificationQueryStrategy<Parent>), additionalQueryData: additionalQueryData);

            //public IQueryable<T> GetEntities<T>(params IQueryStrategy[] queryStrategy);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(reverseStrategy, reverseStrategy2), recorder, specificationStrategy: typeof(DefaultSpecificationQueryStrategy<Parent>), queryStrategy: typeof(AggregateQueryStrategy));

            //public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(isabelleExpression), recorder, expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(isabelleExpression, reverseStrategy, reverseStrategy2), recorder, queryStrategy: typeof(AggregateQueryStrategy), expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(ParentExpressionSpecificationStrategy), recorder, expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(ParentExpressionSpecificationStrategy, reverseStrategy, reverseStrategy2), recorder, queryStrategy: typeof(AggregateQueryStrategy), expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(ParentExpressionSpecificationStrategy, reverseStrategy, reverseStrategy2), recorder, queryStrategy: typeof(AggregateQueryStrategy), expectedCount: 1);

        }

        [Test]
        public void CheckEntitySearchRepositoryExceptionThrown()
        {
            var recorder = new InMemoryRecordedRepository(Persons);
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(p => p.FirstName == "NotFound"), recorder, expectException: true, defaultQueryStrategy: typeof(AggregateQueryStrategy));
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
            InMemoryRecordedRepository recorder,
            Type specificationStrategy = null,
            Type queryStrategy = null,
            bool throwSearchException = true,
            object additionalQueryData = null,
            bool expectException = false,
            Type defaultQueryStrategy = null)
        {
            if (defaultQueryStrategy == null)
                defaultQueryStrategy = typeof(DefaultQueryStrategy);

            var person = default(Person);
            try
            {
                person = getEntity.Invoke();
                person.Id.ShouldEqual(Persons.Cast<Person>().Single(p => p.Id == Names.IsabelleOsborne).Id);
                expectException.ShouldEqual(false);
            }
            catch (EntitySearchRepositoryException)
            {
                expectException.ShouldEqual(true);
            }

            var qEvent = (SimpleRepositoryQueryEvent)recorder.QueryRepository.QueryEvents.Single();
            qEvent.ThrowExceptionIfZeroOrManyFound.ShouldEqual(throwSearchException);
            //            qEvent.QueryStrategy.GetType().ShouldEqual(queryStrategy ?? defaultQueryStrategy);
            qEvent.AdditionalQueryData.ShouldEqual(additionalQueryData);

            recorder.QueryRepository.QueryEvents.Clear();
            return person;
        }

        private IEnumerable<Person> CallAndAssertGetEntities(
            Func<IQueryable<Person>> getEntities,
            InMemoryRecordedRepository recorder,
            Type specificationStrategy = null,
            Type queryStrategy = null,
            object additionalQueryData = null,
            int expectedCount = 6)
        {
            var persons = default(IEnumerable<Person>);

            persons = getEntities.Invoke().ToList();
            persons.Count().ShouldEqual(expectedCount);

            var qEvent = (SimpleRepositoryQueryEvent)recorder.QueryRepository.QueryEvents.Single();
            //            qEvent.QueryStrategy.GetType().ShouldEqual(queryStrategy ?? typeof(DefaultQueryStrategy));
            qEvent.AdditionalQueryData.ShouldEqual(additionalQueryData);

            recorder.QueryRepository.QueryEvents.Clear();
            return persons;
        }
    }
}
