using System;
using System.Collections.Generic;

namespace Jasily.Text.Test
{
    public class TestStringSegmentBase
    {
        protected static IEnumerable<Tuple<string, StringSegment>> Samples()
        {
            yield return Tuple.Create<string, StringSegment>(null, new StringSegment(null));
            yield return Tuple.Create(string.Empty, new StringSegment(string.Empty));
            yield return Tuple.Create("x", new StringSegment("x"));

            yield return Tuple.Create(string.Empty, new StringSegment(string.Empty, 0, 0));
            yield return Tuple.Create(string.Empty, new StringSegment("x", 0, 0));
            yield return Tuple.Create(string.Empty, new StringSegment("x", 1, 0));

            yield return Tuple.Create("1455", new StringSegment("1455"));
            yield return Tuple.Create("455", new StringSegment("1455", 1, 3));

            yield return Tuple.Create("gaf1", new StringSegment("rewgaf1", 3, 4));
        }
    }
}