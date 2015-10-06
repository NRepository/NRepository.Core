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

            // public T GetEntity<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(new TakeQueryStrategy(1)), recorder, expectException: false);

            // public T GetEntity<T>(IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
            CallAndAssertGetEntity(() => recorder.GetEntity<Person>(new TakeQueryStrategy(1), false), recorder, throwSearchException: false);
        }

        //[Test]
        //public void GetEntityWithadditionalQueryData()
        //{
        //    Expression<Func<Person, bool>> isabelleExpression = p => p.Id == Names.IsabelleOsborne;
        //    var additionalQueryData = "DummyData";

        //    var recorder = new InMemoryRecordedQueryRepository(Persons);

        //    //recorder.GetEntity<Person>(object additionalQueryData, Expression<Func<T, bool>> predicate);
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, isabelleExpression), recorder, additionalQueryData: additionalQueryData, defaultQueryStrategy: typeof(AggregateQueryStrategy));

        //    //recorder.GetEntity<Person>(object additionalQueryData, ISpecificationQueryStrategy<T> specification);
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, ExpressionSpecificationStrategy), recorder, additionalQueryData: additionalQueryData);

        //    //recorder.GetEntity<Person>(object additionalQueryData, Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound);
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, ExpressionSpecificationStrategy, false), recorder, throwSearchException: false, additionalQueryData: additionalQueryData);

        //    //recorder.GetEntity<Person>(object additionalQueryData, Expression<Func<T, bool>> predicate, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, isabelleExpression, new OrderByQueryStrategy<Person>(), false), recorder, queryStrategy: typeof(OrderByQueryStrategy<Person>), throwSearchException: false, additionalQueryData: additionalQueryData);

        //    //recorder.GetEntity<Person>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, isabelleExpression, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, queryStrategy: typeof(AggregateQueryStrategy), additionalQueryData: additionalQueryData);

        //    //recorder.GetEntity<Person>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, bool throwExceptionIfZeroOrManyFound);
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, ExpressionSpecificationStrategy, false), recorder, throwSearchException: false, additionalQueryData: additionalQueryData);

        //    //recorder.GetEntity<Person>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound);
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, ExpressionSpecificationStrategy, new OrderByQueryStrategy<Person>(), false), recorder, queryStrategy: typeof(OrderByQueryStrategy<Person>), throwSearchException: false, additionalQueryData: additionalQueryData);

        //    //recorder.GetEntity<Person>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, ExpressionSpecificationStrategy, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, queryStrategy: typeof(AggregateQueryStrategy), additionalQueryData: additionalQueryData);

        //    // public T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) 
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, p => p.Id == Names.IsabelleOsborne), recorder, additionalQueryData: additionalQueryData, defaultQueryStrategy: typeof(AggregateQueryStrategy));

        //    // public T GetEntity<T>(object additionalQueryData, params IQueryStrategy[] queryStrategies) where T : class
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, specificationStrategy: DefaultSpecificationStrategyType, queryStrategy: typeof(AggregateQueryStrategy), expectException: true, additionalQueryData: additionalQueryData);

        //    // public T GetEntity<T>(object additionalQueryData, IQueryStrategy queryStrategy, bool throwExceptionIfZeroOrManyFound) where T : class
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, new TakeQueryStrategy(1), false), recorder, specificationStrategy: DefaultSpecificationStrategyType, queryStrategy: typeof(TakeQueryStrategy), expectException: false, additionalQueryData: additionalQueryData, throwSearchException: false);

        //    // public T GetEntity<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, bool throwExceptionIfZeroOrManyFound) where T : class
        //    CallAndAssertGetEntity(() => recorder.GetEntity<Person>(additionalQueryData, p => p.Id == Names.IsabelleOsborne, false), recorder, additionalQueryData: additionalQueryData, throwSearchException: false);
        //}

        [Test]
        public void GetEntities()
        {
            Expression<Func<Parent, bool>> isabelleExpression = p => p.Id == Names.IsabelleOsborne;
            // var additionalQueryData = "DummyData";

            var recorder = new InMemoryRecordedQueryRepository(Persons);
            var reverseStrategy = new ReverseQueryStrategy();
            var reverseStrategy2 = new ReverseQueryStrategy();

            //public IQueryable<T> GetEntities<T>();
            CallAndAssertGetEntities(() => recorder.GetEntities<Person>(), recorder, expectedCount: 11);
            CallAndAssertGetEntities(() => recorder.GetEntities<Child>(), recorder, expectedCount: 5);

            //public IQueryable<T> GetEntities<T>(object additionalQueryData);
            // CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(additionalQueryData), recorder, specificationStrategy: typeof(DefaultSpecificationQueryStrategy<Parent>), additionalQueryData: additionalQueryData);

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

            //public IQueryable<T> GetEntities<T>(object additionalQueryData, params IQueryStrategy[] queryStrategy);
            //CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(additionalQueryData, reverseStrategy, reverseStrategy2), recorder, specificationStrategy: typeof(DefaultSpecificationQueryStrategy<Parent>), queryStrategy: typeof(AggregateQueryStrategy), additionalQueryData: additionalQueryData);

            ////public IQueryable<T> GetEntities<T>(object additionalQueryData, Expression<Func<T, bool>> predicate);
            //CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(additionalQueryData, isabelleExpression), recorder, additionalQueryData: additionalQueryData, expectedCount: 1);

            ////public IQueryable<T> GetEntities<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies);
            //CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(additionalQueryData, reverseStrategy, reverseStrategy2), recorder, specificationStrategy: typeof(DefaultSpecificationQueryStrategy<Parent>), queryStrategy: typeof(AggregateQueryStrategy), additionalQueryData: additionalQueryData);

            ////public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification);
            //CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(additionalQueryData, ParentExpressionSpecificationStrategy), recorder, additionalQueryData: additionalQueryData, expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(ParentExpressionSpecificationStrategy, reverseStrategy, reverseStrategy2), recorder, queryStrategyType: typeof(AggregateQueryStrategy), expectedCount: 1);

            //public IQueryable<T> GetEntities<T>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, IQueryStrategy queryStrategy);
            //CallAndAssertGetEntities(() => recorder.GetEntities<Parent>(additionalQueryData, ParentExpressionSpecificationStrategy, reverseStrategy), recorder, queryStrategy: typeof(ReverseQueryStrategy), expectedCount: 1, additionalQueryData: additionalQueryData);

            ////recorder.GetEntity<Person>(object additionalQueryData, ISpecificationQueryStrategy<T> specification, params IQueryStrategy[] queryStrategies);
            //CallAndAssertGetEntities(() => recorder.GetEntities<Person>(additionalQueryData, ExpressionSpecificationStrategy, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, specificationStrategy: ExpressionSpecificationStrategy.GetType(), queryStrategy: typeof(AggregateQueryStrategy), additionalQueryData: additionalQueryData, expectedCount: 1);

            //// public IQueryable<T> GetEntities<T>(object additionalQueryData, Expression<Func<T, bool>> predicate, params IQueryStrategy[] queryStrategies) where T : class
            //CallAndAssertGetEntities(() => recorder.GetEntities<Person>(additionalQueryData, p => p.Id == Names.IsabelleOsborne, new OrderByQueryStrategy<Person>(), new OrderByQueryStrategy<Person>()), recorder, specificationStrategy: ExpressionSpecificationStrategy.GetType(), queryStrategy: typeof(AggregateQueryStrategy), additionalQueryData: additionalQueryData, expectedCount: 1);

            // IQueryable<T> GetQueryableEntities<T>(object additionalQueryData) where T : class
            // CallAndAssertGetEntities(() => recorder.GetQueryableEntities<Person>(additionalQueryData), recorder, specificationStrategy: typeof(DefaultSpecificationQueryStrategy<Person>), queryStrategy: typeof(DefaultQueryStrategy), additionalQueryData: additionalQueryData, expectedCount: 11);
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
