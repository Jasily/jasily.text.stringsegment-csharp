using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable ObjectCreationAsStatement

namespace Jasily.Text.Test
{
    [TestClass]
    public class TestStringSegmentConstructor
    {
        [TestMethod]
        public void TestConstructor()
        {
            new StringSegment(null);
            new StringSegment(string.Empty);
            new StringSegment("x");

            new StringSegment(string.Empty, 0);
            new StringSegment("x", 0);
            new StringSegment("x", 1);

            new StringSegment(string.Empty, 0, 0);
            new StringSegment("x", 0, 0);
            new StringSegment("x", 0, 1);
            new StringSegment("x", 1, 0);
        }

        [TestMethod]
        public void TestConstructorArgumentNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new StringSegment(null, 0));
            Assert.ThrowsException<ArgumentNullException>(() => new StringSegment(null, 0, 0));
        }

        [TestMethod]
        public void TestConstructorArgumentOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StringSegment(string.Empty, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StringSegment("x", 2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StringSegment(string.Empty, 1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StringSegment(string.Empty, 0, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StringSegment("x", 2, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StringSegment("x", 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StringSegment("x", 0, 2));
        }
    }
}