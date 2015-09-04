namespace NUnit.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class NUnitExtensions
    {
        [DebuggerStepThrough]
        public static void ShouldEqual(this object actualValue, object expectedValue)
        {
            ShouldEqual(actualValue, expectedValue, string.Empty);
        }

        [DebuggerStepThrough]
        public static void ShouldEqual(this object actualValue, object expectedValue, string message)
        {
            if (expectedValue == null)
            {
                Assert.IsNull(actualValue, message);
                return;
            }

            Assert.AreEqual(expectedValue, actualValue, message);
        }

        [DebuggerStepThrough]
        public static void ShouldNotEqual(this object actualValue, object expectedValue)
        {
            ShouldNotEqual(actualValue, expectedValue, string.Empty);
        }

        [DebuggerStepThrough]
        public static void ShouldNotEqual(this object actualValue, object expectedValue, string message)
        {
            if (expectedValue == null)
            {
                Assert.IsNotNull(actualValue, message);
                return;
            }

            Assert.AreNotEqual(expectedValue, actualValue, message);
        }

        [DebuggerStepThrough]
        public static void ShouldNotContain(this IEnumerable<object> items, object expectedItem)
        {
            ShouldEqual(items, expectedItem, string.Empty);
        }

        [DebuggerStepThrough]
        public static void ShouldNotContain(this IEnumerable<object> items, object expectedItem, string message)
        {
            if (expectedItem == null)
            {
                Assert.IsNull(items, message);
                return;
            }

            var contains = items.Contains(expectedItem);
            if (!contains)
                return;

            if (string.IsNullOrWhiteSpace(message))
                Assert.Fail(string.Format("Items should not contain: {0}", expectedItem));
            else
                Assert.Fail(message);
        }

        [DebuggerStepThrough]
        public static void ShouldContain(this IEnumerable<object> items, object expectedItem)
        {
            ShouldEqual(items, expectedItem, string.Empty);
        }

        [DebuggerStepThrough]
        public static void ShouldContain(this IEnumerable<object> items, object expectedItem, string message)
        {
            if (expectedItem == null)
            {
                Assert.IsNull(items, message);
                return;
            }

            Assert.Contains(expectedItem, items.ToList());
        }
    }
}
