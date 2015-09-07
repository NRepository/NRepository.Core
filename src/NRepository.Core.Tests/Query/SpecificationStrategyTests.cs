using NRepository.Core.Query;
using NRepository.Core.Query.Specification;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BluePear.Repository.CoreTests.Query
{
    [TestFixture]
    public class SpecificationStrategyTests
    {
        private class StringSearchQueryStrategy : QueryStrategy
        {
            public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
            {
                var list = (List<string>)additionalQueryData;
                list.Add("HelloFromQueryStratefy");
                return (IQueryable<T>)this.QueryableRepository.GetQueryableEntities<T>(additionalQueryData);
            }
        }

        private class StringSearchSpecificationQueryStrategy : SpecificationQueryStrategy<string>
        {

            public override System.Linq.Expressions.Expression<Func<string, bool>> SatisfiedBy(object additionalQueryData)
            {
                var list = (List<string>)additionalQueryData;
                list.Add("Hello");
                Debug.Print("additionalQueryData {0}", list.Count);

                return p => true;
            }
        }

        [Test]
        public void CheckAdditionQueryStrategies()
        {
            var repository = new InMemoryQueryRepository(new[] { "1", "2", "3", "4", "5", "6", "7", "8" });

            var counter = new List<string>();
            repository.GetEntities<string>(
                counter,
                new StringSearchSpecificationQueryStrategy() &
                new StringSearchSpecificationQueryStrategy() &
                (new StringSearchSpecificationQueryStrategy() | new StringSearchSpecificationQueryStrategy()) & new StringSearchSpecificationQueryStrategy() & new StringSearchSpecificationQueryStrategy(),
                new StringSearchQueryStrategy()
                );

            counter.Count().ShouldEqual(7);
        }
    }
}
