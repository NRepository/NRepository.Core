//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NRepository.Core.Query;
//using NUnit.Framework;
//using NRepository.Core.Tests;
//using NRepository.TestHelpers;
//namespace NRepository.Core.Query.Tests
//{
//    [TestFixture()]
//    public class OrderByDescendingQueryStrategyTests
//    {
//        private const string IdString = "Id";
//        private const string GroupIdString = "GroupId";

//        [Test]
//        public void CheckConstructors()
//        {
//            var query = new OrderByDescendingQueryStrategy(IdString);
//            query.PropertyNames.First().ShouldEqual(IdString);

//            var query2 = new OrderByDescendingQueryStrategy(IdString, "1", "2");
//            query2.PropertyNames.First().ShouldEqual(IdString);
//            query2.PropertyNames.Count().ShouldEqual(2);
//            query2.PropertyNames.Skip(1).First().ShouldEqual("1");
//            query2.PropertyNames.Last().ShouldEqual("2");
//        }

//        [Test]
//        public void OrderByDescendingQueryStrategyTest()
//        {
//            // Arrange
//            var simpleEntities = SimpleEntity.CreateSimpleEntities();
//            var query = new OrderByDescendingQueryStrategy(IdString);
//            
//            // Act
//            var results = simpleEntities.AddQueryStrategy(query).ToList();

//            // Assert
//            results.Count().ShouldEqual(simpleEntities.Count());
//            results.First().Id.ShouldEqual(simpleEntities.Last().Id);
//        }

//        [Test]
//        public void OrderByDescendingQueryStrategyTest1()
//        {
//            // Arrange
//            var simpleEntities = SimpleEntity.CreateSimpleEntities();
//            var query = new OrderByDescendingQueryStrategy(GroupIdString, IdString);

//            // Act
//            var results = simpleEntities.AddQueryStrategy(query).ToList();

//            // Assert
//            results.First().Id.ShouldEqual(7);
//        }
//    }
//}
