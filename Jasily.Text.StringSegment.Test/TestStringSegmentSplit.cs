using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Text.Test
{
    [TestClass]
    public class TestStringSegmentSplit : TestStringSegmentBase
    {
        private static IEnumerable<int> Count()
        {
            return Enumerable.Range(-1, 5);
        }

        private static IEnumerable<StringSplitOptions> Options()
        {
            return (StringSplitOptions[]) Enum.GetValues(typeof(StringSplitOptions));
        }
        
        [TestMethod]
        public void TestSplitChars()
        {
            IEnumerable<char[]> Separators()
            {
                yield return null;
                yield return new char[0];
                yield return new[] { '4' };
                yield return new[] { '4', '5' };
            }

            foreach (var sample in Samples())
            {
                var str = sample.Item1;
                var ss = sample.Item2;

                foreach (var count in Count())
                {
                    foreach (var option in Options())
                    {
                        foreach (var separator in Separators())
                        {
                            if (str == null)
                            {
                                Assert.ThrowsException<InvalidOperationException>(() =>
                                {
                                    if (count == -1)
                                    {
                                        ss.Split(separator, option);
                                    }
                                    else
                                    {
                                        ss.Split(separator, count, option);
                                    }
                                });
                            }
                            else if (count == -1)
                            {
                                var native = str.Split(separator, option);
                                var segment = ss.Split(separator, option).Select(z => z.ToString()).ToArray();
                                CollectionAssert.AreEqual(native, segment);
                            }
                            else
                            {
                                var native = str.Split(separator, count, option);
                                var segment = ss.Split(separator, count, option).Select(z => z.ToString()).ToArray();
                                CollectionAssert.AreEqual(native, segment);
                            }
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void TestSplitStrings()
        {
            IEnumerable<string[]> Separators()
            {
                yield return null;
                yield return new string[0];
                yield return new[] { "" };
                yield return new[] { "4" };
                yield return new[] { "4", "5" };
            }

            foreach (var sample in Samples())
            {
                var str = sample.Item1;
                var ss = sample.Item2;

                foreach (var count in Count())
                {
                    foreach (var option in Options())
                    {
                        foreach (var separator in Separators())
                        {
                            if (str == null)
                            {
                                Assert.ThrowsException<InvalidOperationException>(() =>
                                {
                                    if (count == -1)
                                    {
                                        ss.Split(separator, option);
                                    }
                                    else
                                    {
                                        ss.Split(separator, count, option);
                                    }
                                });
                            }
                            else if (count == -1)
                            {
                                var native = str.Split(separator, option);
                                var segment = ss.Split(separator, option).Select(z => z.ToString()).ToArray();
                                CollectionAssert.AreEqual(native, segment);
                            }
                            else
                            {
                                var native = str.Split(separator, count, option);
                                var segment = ss.Split(separator, count, option).Select(z => z.ToString()).ToArray();
                                CollectionAssert.AreEqual(native, segment);
                            }
                        }
                    }
                }
            }
        }
    }
}