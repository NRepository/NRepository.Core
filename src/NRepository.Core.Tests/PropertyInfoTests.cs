namespace NRepository.Core.Tests
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture()]
    public class PropertyInfoTests
    {
        [Test]
        public void GetPropertyNameTest()
        {
            var include = PropertyInfo<Parent>.GetMemberName(p => p.Children);
            include.ShouldEqual("Children"); 

            include = PropertyInfo<Parent>.GetMemberName(p => p.Children.Select(p1 => p1.Partner));
            include.ShouldEqual("Children.Partner");

            include = PropertyInfo<Parent>.GetMemberName(p => p.Children.Select(p1 => p1.Partner.Children));
            include.ShouldEqual("Children.Partner.Children"); 

            include = PropertyInfo<Parent>.GetMemberName(p => p.Children.Select(p1 => p1.Partner.Children.Select(p3 => p3.Partner)));
            include.ShouldEqual("Children.Partner.Children.Partner");

            include = PropertyInfo<Parent>.GetMemberName(p => p.Partner.Children.Select(p2 => p2.Partner.Children.Select(p3 => p3.Pet)));
            include.ShouldEqual("Partner.Children.Partner.Children.Pet");

            include = PropertyInfo<Parent>.GetMemberName(p => p.Partner.Children.Select(p2 => p2.Partner.Children.Select(p3 => p3.Pet.Id)));
            include.ShouldEqual("Partner.Children.Partner.Children.Pet.Id");
            
            include = PropertyInfo<Parent>.GetMemberName(p => p.Id);
            include.ShouldEqual("Id");

            include = PropertyInfo<Parent>.GetMemberName(p => p.IsFemale);
            include.ShouldEqual("IsFemale");
        }
    }
}
