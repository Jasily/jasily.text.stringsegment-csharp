using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Text.Test
{
    [TestClass]
    public class TestStringSegmentProperties : TestStringSegmentBase
    {
        [TestMethod]
        public void TestNullStringSegment()
        {
            foreach (var sample in Samples())
            {
                var text = sample.Item1;
                var segment = sample.Item2;

                if (text == null)
                {
                    Assert.IsNull(segment.Buffer);
                    Assert.AreEqual(0, segment.Offset);
                    Assert.AreEqual(0, segment.Length);
                    Assert.AreEqual(false, segment.HasValue);
                    Assert.AreEqual(null, segment.Value);
                    Assert.AreEqual(string.Empty, segment.ToString());
                }
                else
                {
                    Assert.IsNotNull(segment.Buffer);
                    Assert.IsTrue(segment.Offset >= 0);
                    Assert.IsTrue(segment.Length >= 0);
                    Assert.AreEqual(text.Length, segment.Length);
                    Assert.IsTrue(segment.Offset + segment.Length <= segment.Buffer.Length);
                    Assert.AreEqual(true, segment.HasValue);
                    Assert.AreEqual(text, segment.Value);
                    Assert.AreEqual(text, segment.ToString());
                }
            }
        }
    }
}
