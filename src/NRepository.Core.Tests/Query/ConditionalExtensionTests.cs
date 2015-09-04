namespace NRepository.Core.Tests.Query
{
    using NRepository.Core.Query;
    using NRepository.Core.Query.Specification;
    using NUnit.Framework;

    [TestFixture]
    public class ConditionalExtensionTests
    {
        [Test]
        public void CheckStringOnCondition()
        {
            var dummyString = "dummy";
            ConditionalExtensions.OnCondition(dummyString, true).ShouldEqual(dummyString);
            ConditionalExtensions.OnCondition(dummyString, false).ShouldEqual(null);
        }

        [Test]
        public void CheckQueryStrategyOnCondition()
        {
            var strategy = new ReverseQueryStrategy();
            ConditionalExtensions.OnCondition(strategy, true).GetType().ShouldEqual(typeof(ReverseQueryStrategy));
            ConditionalExtensions.OnCondition(strategy, false).GetType().ShouldEqual(typeof(DefaultQueryStrategy));
        }

        [Test]
        public void CheckSpecificationQueryStrategyOnCondition()
        {
            var strategy = new ExpressionSpecificationQueryStrategy<SimpleEntity>(p => p.Id == 0);
            ConditionalExtensions.OnCondition(strategy, true).GetType().ShouldEqual(typeof(ExpressionSpecificationQueryStrategy<SimpleEntity>));
            ConditionalExtensions.OnCondition(strategy, false).GetType().ShouldEqual(typeof(DefaultSpecificationQueryStrategy<SimpleEntity>));
        }
    }
}
