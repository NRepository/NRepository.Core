namespace NRepository.Core.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using NRepository.Core.Query;

    [TestFixture]
    public class InterceptorTests
    {
        [Test]
        public void CheckEmptyRepositoryDoesNotIncludeFamilyView()
        {
            var repositoryViews = new FamilyQueryRepository();

            var entities = repositoryViews.GetEntities<PersonProjection>();

            entities.Count().ShouldEqual(0);
        }

        [Test]
        public void CheckSimpleInterceptor()
        {
            var repositoryViews = new FamilyQueryRepository(new AdditionalViewsInterceptor());

            // Act
            var entities = repositoryViews.GetEntities<PersonProjection>();
            entities.Count().ShouldEqual(11);
            entities.Any(p => p.AdditionalQueryData != null).ShouldEqual(false);
        }
    }
}