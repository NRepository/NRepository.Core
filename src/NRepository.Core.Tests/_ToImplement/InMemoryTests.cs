namespace NRepository.Core.Tests
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class InMemoryTests
    {
        [Test]
        public void AllTests()
        {
//            tmp.GetEntities<string>()
            
                // don't use nreopsitory test kit here
            //var repository = new InMemoryRecordedRepository();
            //repository.Add("ddd");
            //"abcdefghi".ToList().ForEach(p => repository.Add(p.ToString()));//

            //var entities = repository.GetEntities<string>(new PagingQueryStrategy(3, 3));
            //entities.ToList().ForEach(p =>
            //{
            //    repository.Delete(p);
            //});

            //repository.Save();
            //Assert.AreEqual(10, repository.CommandRepository.CommandEvents.AddedEvents.Count);
            //Assert.AreEqual(3, repository.CommandRepository.CommandEvents.DeletedEvents.Count);
            //Assert.AreEqual(1, repository.CommandRepository.CommandEvents.SavedEvents.Count);
            //Assert.AreEqual(1, repository.QueryRepository.QueryEvents.Count);
        }

        [Test]
        public void ConditionalQueryStrategyTest()
        {
            //var repository = new InMemoryRecordedRepository();
            //"abcdefghi".ToList().ForEach(p => repository.Add(p.ToString()));

            //bool predicate = true;
            //var entities = repository.GetEntities<string>(new AggregateQueryStrategy(
            //    new ConditionalQueryStrategy(predicate, new ReverseQueryStrategy()),
            //    new ConditionalQueryStrategy(!predicate, new TakeQueryStrategy(3))
            //    )).ToList();

            //entities.Count().ShouldEqual(9);
            //entities.First().ShouldEqual("i");

            //predicate = false;
            //entities = repository.GetEntities<string>(new AggregateQueryStrategy(
            //    new ConditionalQueryStrategy(predicate, new ReverseQueryStrategy()),
            //    new ConditionalQueryStrategy(!predicate, new TakeQueryStrategy(3))
            //    )).ToList();

            //entities.Count().ShouldEqual(3);
            //entities.First().ShouldEqual("a");
        }

        [Test]
        public void ConditionalAggregateQueryStrategyTest()
        {
            //var repository = new InMemoryRecordedRepository();
            //"abcdefghi".ToList().ForEach(p => repository.Add(p.ToString()));

            //bool predicate = true;
            //var entities = repository.GetEntities<string>(
            //    new ConditionalAggregateQueryStrategy(predicate,
            //        new ReverseQueryStrategy(),
            //        new TakeQueryStrategy(1)))
            //        .ToList();

            //entities.Count().ShouldEqual(1);
            //entities.First().ShouldEqual("i");

            //predicate = false;
            //entities = repository.GetEntities<string>(
            //    new ConditionalAggregateQueryStrategy(predicate,
            //        new ReverseQueryStrategy(),
            //        new TakeQueryStrategy(1)))
            //        .ToList();

            //entities.Count().ShouldEqual(9);
            //entities.First().ShouldEqual("a");
        }

        [Test]
        public void ConditionalExtensionQueryTest()
        {
            //var repository = new InMemoryRecordedRepository();
            //"abcdefghi".ToList().ForEach(p => repository.Add(p.ToString()));

            //bool predicate = true;
            //var entities = repository.GetEntities<string>(new AggregateQueryStrategy(
            //    new ReverseQueryStrategy().OnCondition(predicate),
            //    new TakeQueryStrategy(3).OnCondition(!predicate)
            //    )).ToList();

            //entities.Count().ShouldEqual(9);
            //entities.First().ShouldEqual("i");

            //predicate = false;
            //entities = repository.GetEntities<string>(new AggregateQueryStrategy(
            //    new ReverseQueryStrategy().OnCondition(predicate),
            //    new TakeQueryStrategy(3).OnCondition(!predicate)
            //    )).ToList();

            //entities.Count().ShouldEqual(3);
            //entities.First().ShouldEqual("a");
        }
    }
}
